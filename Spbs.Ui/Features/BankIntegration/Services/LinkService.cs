using System;
using System.Threading.Tasks;
using Integrations.Nordigen;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public class LinkService : ILinkService
{
    private readonly INordigenLinkWriterRepository _linkWriterRepository;
    private readonly INordigenApiClient _nordigenCLient;
    private readonly IRedirectLinkService _linkService;

    public record struct RedirectUrl(string Url);

    public LinkService(INordigenLinkWriterRepository linkWriterRepository, INordigenApiClient nordigenCLient, IRedirectLinkService linkService)
    {
        _linkWriterRepository = linkWriterRepository;
        _nordigenCLient = nordigenCLient;
        _linkService = linkService;
    }
    
    public async Task<RedirectUrl?> CreateLink(Institution institution, NordigenEula eula, Guid userId, bool accountSelection)
    {
        NordigenLink? link = new NordigenLink
        {
            InstitutionId = institution.Id,
            Agreement = eula.Id,
            UserId = userId,
            UserLanguage = "EN"
        };

        link = await _linkWriterRepository.Upsert(link);
        if (link is null)
        {
            // Todo: handle errors
            return null;
        }

        string redirect = _linkService.GetUrlForAccountListing();
        string reference = link.Id.ToString(); //redirect-url?ref={reference} 
        
        var requisition = await _nordigenCLient.CreateRequisition(redirect, institution.Id, eula.Id, reference, accountSelection);
        if (requisition is null)
        {
            // TODO: Handle error
            return null;
        }

        return new RedirectUrl(requisition!.Redirect);
    }
}

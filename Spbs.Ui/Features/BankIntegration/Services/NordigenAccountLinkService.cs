using System;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public class NordigenAccountLinkService : INordigenAccountLinkService
{
    private readonly INordigenLinkWriterRepository _linkWriterRepository;
    private readonly INordigenApiClient _nordigenCLient;
    private readonly IRedirectLinkService _linkService;
    private readonly IMapper _mapper;

    public record struct RedirectUrl(string Url);

    public NordigenAccountLinkService(INordigenLinkWriterRepository linkWriterRepository, INordigenApiClient nordigenCLient, IRedirectLinkService linkService, IMapper mapper)
    {
        _linkWriterRepository = linkWriterRepository;
        _nordigenCLient = nordigenCLient;
        _linkService = linkService;
        _mapper = mapper;
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
        
        string redirect = _linkService.GetUrlForNordigenRedirect();
        string reference = link.Id.ToString(); //redirect-url?ref={reference} 
        
        var requisition = await _nordigenCLient.CreateRequisition(redirect, institution.Id, eula.Id, reference, accountSelection);
        if (requisition is null)
        {
            // TODO: Handle error
            return null;
        }

        link = _mapper.Map<NordigenLink>(requisition);
        await _linkWriterRepository.Upsert(link);
        
        return new RedirectUrl(requisition!.Link);
    }

    public async Task DeleteLink(NordigenLink link)
    {
        if (link.NordigenId is { } nordigenId)
        {
            await _nordigenCLient.DeleteRequisition(link.NordigenId.Value);
        }

        await _linkWriterRepository.Delete(link);
    }
}

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinksOverviewPage : SelectableListComponent<NordigenLink>
{
    [Inject] private INordigenLinkReaderRepository _linkReader { get; set; }
    [Inject] private INordigenApiClient _nordigenCLient { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    
    protected override List<NordigenLink>? GetList() => _userLinks.ToList();
    
    private ReadOnlyCollection<NordigenLink>? _userLinks;
    private ReadOnlyCollection<Institution>? _institutions;
    
    protected override async Task OnInitializedAsync()
    {
        await InitInstitutionList();
        await GetLinksForUser();
        StateHasChanged();
    }
    
    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
    }

    private async Task GetLinksForUser()
    {
        Guid? userId = await UserId();
        _userLinks = await _linkReader.GetLinksForUser(userId!.Value);
    }

    private async Task InitInstitutionList()
    {
        // TODO: Remove hard coded country
        var aspsps = await _nordigenCLient.GetListOfInstitutionsAsync("SE");
        var institutions = _mapper.Map<List<Institution>>(aspsps);
        
#if DEBUG
        institutions.Add(new Institution { Name = "Sandbox", Id = "SANDBOXFINANCE_SFIN0000", Bic = string.Empty });
#endif
        
        _institutions = new(institutions);
    }
}
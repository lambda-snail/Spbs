using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.NewLink;

public partial class InstitutionSelectorComponent : SelectableListComponent<Institution>
{
    private string _country = "se";
    private List<Institution>? _institutions = null;
    protected override List<Institution>? GetList() => _institutions;
    
    [Inject] public IMapper Mapper { get; set; }
    [Inject] public INordigenApiClient Client { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetListOfInstitutions();
    }

    private async Task GetListOfInstitutions()
    {
        var aspsps = await Client.GetListOfInstitutionsAsync(_country);
        _institutions = Mapper.Map<List<Institution>>(aspsps);
     
#if DEBUG
        _institutions.Add(new Institution { Name = "Sandbox", Id = "SANDBOXFINANCE_SFIN0000"});
#endif
        
        StateHasChanged();
    }
    
    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
    }

    public Institution? GetSelectedInstitution()
    {
        int? i = GetSelected();
        if (i is null || _institutions is null)
        {
            return null;
        }

        return _institutions[i.Value];
    }
}
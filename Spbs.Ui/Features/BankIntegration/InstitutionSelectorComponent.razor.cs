using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public partial class InstitutionSelectorComponent
{
    private string _country = "se";
    private List<Institution>? _institutions = null;

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
        StateHasChanged();
    }
}
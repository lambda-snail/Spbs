using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public partial class BankSyncronizationPage
{
    private string _country = "se";
    private List<Institution>? _institutions = null;

    [Inject] public IMapper Mapper { get; set; }
    [Inject] public NordigenApiClient Client { get; set; }

    public BankSyncronizationPage() { }

    protected override void OnInitialized()
    {
        
    }

    private async Task GetListOfInstitutions()
    {
        var aspsps = await Client.GetListOfInstitutionsAsync(_country);
        _institutions = Mapper.Map<List<Institution>>(aspsps);
        StateHasChanged();
    }
}
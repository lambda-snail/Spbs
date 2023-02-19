using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.BankIntegration;

public partial class BankSyncronizationPage
{
    [Inject] public NordigenApiClient Client { get; set; }

    public BankSyncronizationPage() { }

    protected override void OnInitialized()
    {
        
    }
}
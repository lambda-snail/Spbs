using Integrations.Nordigen;

namespace Spbs.Ui.Features.BankIntegration;

public partial class BankSyncronizationPage
{
    private readonly NordigenApiClient _client;

    public BankSyncronizationPage(NordigenApiClient client)
    {
        _client = client;
    }
}
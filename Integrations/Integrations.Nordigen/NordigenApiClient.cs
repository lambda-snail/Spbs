using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Integrations.Nordigen;

public class NordigenApiClient
{
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;
    private readonly NordigenTokenClient _tokenClient;
    private readonly IConfidentialClientApplication _application;
    
    public NordigenApiClient(HttpClient client, IOptions<NordigenOptions> options, NordigenTokenClient tokenClient)
    {
        _client = client;
        _options = options;
        _tokenClient = tokenClient;

        _client.BaseAddress = new Uri(options.Value.ServiceUrl);
    }

    public async Task GetListOfInstitutions()
    {
        var tokens = await _tokenClient.ObtainTokenAsync();
    }
}
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Integrations.Nordigen;

public class NordigenApiClient
{
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;
    private readonly NordigenTokenClient _tokenCLient;
    private readonly IConfidentialClientApplication _application;
    
    public NordigenApiClient(HttpClient client, IOptions<NordigenOptions> options, NordigenTokenClient tokenCLient)
    {
        _client = client;
        _options = options;
        _tokenCLient = tokenCLient;

        _client.BaseAddress = new Uri(options.Value.ServiceUrl);
        var tokens = tokenCLient.GetNewToken();
    }
}
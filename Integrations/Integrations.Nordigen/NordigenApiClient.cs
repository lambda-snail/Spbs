using Integrations.Nordigen.Extensions;
using Integrations.Nordigen.Models;
using Microsoft.Extensions.Options;

namespace Integrations.Nordigen;

public class NordigenApiClient
{
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;
    private readonly NordigenTokenClient _tokenClient;

    public NordigenApiClient(HttpClient client, IOptions<NordigenOptions> options, NordigenTokenClient tokenClient)
    {
        _client = client;
        _options = options;
        _tokenClient = tokenClient;

        _client.BaseAddress = new Uri(options.Value.ServiceUrl);
    }

    /// <summary>
    /// Retrieve the list of supported institutions for the given country, or an empty list if no institutions exist.
    /// </summary>
    public async Task<List<Aspsp>> GetListOfInstitutionsAsync(string country)
    {
        var tokenResult = await _tokenClient.ObtainTokenAsync();
        var token = tokenResult.Token;
        if (token is null)
        {
            return new();
        }

        string queryString = "?country=" + country;
        var response = await _client.SendGetRequest(_options.Value.ListOfInstitutionsEndpoint + queryString, token.Access);
        List<Aspsp>? institutions = await response.ParseResponseAsync<List<Aspsp>>();
        
        institutions ??= new();

        foreach (var institution in institutions)
        {
            Console.WriteLine(institution.Name);
        }        
        return institutions;
    }
    
    public async Task<Aspsp?> GetInstitutionAsync(Guid id)
    {
        var tokenResult = await _tokenClient.ObtainTokenAsync();
        var token = tokenResult.Token;
        if (token is null)
        {
            return null;
        }

        string queryString = string.Format("{0}{1}", _options.Value.ListOfInstitutionsEndpoint, id.ToString());
        var response = await _client.SendGetRequest( queryString, token.Access);
        Aspsp? institution = await response.ParseResponseAsync<Aspsp>();
        return institution;
    }
}
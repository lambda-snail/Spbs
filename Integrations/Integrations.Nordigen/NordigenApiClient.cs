using System.Runtime.Loader;
using Integrations.Nordigen.Extensions;
using Integrations.Nordigen.Models;
using LazyCache;
using Microsoft.Extensions.Options;

namespace Integrations.Nordigen;

public class NordigenApiClient : INordigenApiClient
{
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;
    private readonly NordigenTokenClient _tokenClient;

    private IAppCache _cache;
    private readonly string _tokenCacheString = "__token";
    
    public NordigenApiClient(HttpClient client, IOptions<NordigenOptions> options, NordigenTokenClient tokenClient, IAppCache cache)
    {
        _client = client;
        _options = options;
        _tokenClient = tokenClient;
        _cache = cache;

        _client.BaseAddress = new Uri(options.Value.ServiceUrl);
    }

    /// <summary>
    /// Retrieve the list of supported institutions for the given country, or an empty list if no institutions exist.
    /// </summary>
    public async Task<List<Aspsp>> GetListOfInstitutionsAsync(string country)
    {
        var token = await GetTokenCached();
        if (token is null)
        {
            return new();
        }

        string queryString = _options.Value.ListOfInstitutionsEndpoint + "?country=" + country;
        var response = await _client.SendGetRequest( queryString, token);
        List<Aspsp>? institutions = await response.ParseResponseAsync<List<Aspsp>>();
        
        institutions ??= new();
        foreach (var institution in institutions)
        {
            Console.WriteLine(institution.Name);
        }        
        return institutions;
    }

    private async Task<string?> GetTokenCached() // TODO: Handle case when refresh is expired
    {
        var tokenResult = await _cache.GetOrAddAsync(_tokenCacheString,
            async () => await _tokenClient.ObtainTokenAsync()
        );

        var token = tokenResult.Token;
        if (token.HasAccessTokenExpiredAtTime(DateTime.UtcNow.AddMinutes(10)))
        {
            var refreshedToken = await _tokenClient.RefreshTokenAsync(token.Refresh);
            token.UpdateAccessToken(refreshedToken.Token.Access, refreshedToken.Token.AccessExpires);
        }

        return token.Access;
    }

    public async Task<Aspsp?> GetInstitutionAsync(string id)
    {
        var token = await GetTokenCached();
        if (token is null)
        {
            return null;
        }

        string queryString = _options.Value.ListOfInstitutionsEndpoint + id;
        var response = await _client.SendGetRequest(queryString, token);
        Aspsp? institution = await response.ParseResponseAsync<Aspsp>();
        return institution;
    }
}
using System.Runtime.Loader;
using Integrations.Nordigen.Configuration;
using Integrations.Nordigen.Extensions;
using Integrations.Nordigen.Models;
using LazyCache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Integrations.Nordigen;

public class NordigenApiClient : INordigenApiClient
{
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;
    private readonly NordigenTokenClient _tokenClient;
    private readonly ILogger<NordigenApiClient> _logger;

    private IAppCache _cache;
    private readonly string _tokenCacheString = "__token";
    
    public NordigenApiClient(HttpClient client, IOptions<NordigenOptions> options, NordigenTokenClient tokenClient, ILogger<NordigenApiClient> logger, IAppCache cache)
    {
        _client = client;
        _options = options;
        _tokenClient = tokenClient;
        _logger = logger;
        _cache = cache;

        _client.BaseAddress = new Uri(options.Value.ServiceUrl!);
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

    public async Task<EndUserAgreement?> CreateEndUserAgreement(string institutionId, int maxHistoricalDays, int accessValidForDays, List<string> accessScope)
    {
        _logger.LogInformation("Create new agreement {InstitutionId} valid for {HistoricalDays} accessible for {ValidDays}", institutionId, maxHistoricalDays, accessValidForDays);
        
        var request = new EndUserAgreementRequest(maxHistoricalDays, accessValidForDays, accessScope, institutionId);
        if(await GetTokenCached() is not { } token)
        {
            return null; // TODO: Add error handling, use OneOf
        }

        var endpoint = _options.Value.CreateAgreementEndpoint!;
        var response = await _client.SendPostRequest(request, endpoint, token);
        EndUserAgreement? agreement = await response.ParseResponseAsync<EndUserAgreement>();
        if (agreement is null)
        {
            var message = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Request to create agreement for {InstitutionId} failed with code {HttpCode} and message '{Message}'", institutionId, response.StatusCode, message);
            return null;
        }

        return agreement;
    }
}
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
    private static readonly string _tokenCacheString = "__token";
    private static readonly string _institutionListCacheString = "__institutions";
    private static readonly string _accountsPrefixCacheString = "__accounts_";
    
    private static readonly string NordigenFormatString = "yyyy-MM-dd";
    
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
        List<Aspsp> institutions = await _cache.GetOrAddAsync(_institutionListCacheString,
            async () =>
            {
                var token = await GetTokenCached();
                if (token is null)
                {
                    return new List<Aspsp>();
                }

                string queryString = _options.Value.ListOfInstitutionsEndpoint + "?country=" + country;
                var response = await _client.SendGetRequest( queryString, token);
            
                List<Aspsp>? institutions = await response.ParseResponseAsync<List<Aspsp>>();
                institutions ??= new();

                return institutions;
            }
        );

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

    /// <summary>
    /// Creates new link.
    /// </summary>
    /// <param name="redirect">Redirect URL to your application after end-user authorization with ASPSP (required).</param>
    /// <param name="institutionId">An Institution ID for this Requisition (required).</param>
    /// <param name="agreement">EUA associated with this requisition.</param>
    /// <param name="reference">Additional ID to identify the end user.</param>
    /// <param name="userLanguage">A two-letter country code (ISO 639-1).</param>
    /// <param name="ssn">Optional SSN field to verify ownership of the account.</param>
    /// <param name="accountSelection">Option to enable account selection view for the end user (default to false).</param>
    /// <param name="redirectImmediate">Enable redirect back to the client after account list received (default to false).</param>
    public async Task<RequisitionV2?> CreateRequisition(string redirect, string institutionId, Guid agreement, string reference, bool accountSelection, string userLanguage = "EN", bool redirectImmediate = false, string ssn = "")
    {
        _logger.LogInformation("Create new requisition for {InstitutionId}, {AgreementId}, with reference {Reference}", institutionId, agreement, reference);

        var request = new RequisitionV2Request(redirect, institutionId, agreement, reference, userLanguage, ssn,
            accountSelection, redirectImmediate);
        if(await GetTokenCached() is not { } token)
        {
            return null; // TODO: Add error handling, use OneOf
        }
        
        var endpoint = _options.Value.RequisitionEndpoint!;
        var response = await _client.SendPostRequest(request, endpoint, token);
        var requisition = await response.ParseResponseAsync<RequisitionV2>();
        if (requisition is null)
        {
            _logger.LogWarning("Failed to create link for {InstitutionId} with status {HttpCode}", institutionId, response.StatusCode);
            return null;
        }

        // Sometimes Nordigen sends back response with this field empty
        requisition.Agreement ??= agreement;

        return requisition;
    }

    public async Task<RequisitionV2?> GetRequisition(Guid requisitionId)
    {
        _logger.LogInformation("Get requisition {RequisitionId}", requisitionId);
        if(await GetTokenCached() is not { } token)
        {
            return null;
        }
        
        var endpoint = _options.Value.RequisitionEndpoint!;
        endpoint += requisitionId.ToString() + '/';
        var response = await _client.SendGetRequest(endpoint, token);
        
        _logger.LogInformation("Request to get requisition {RequisitionId} returned with status code {StatusCode}", requisitionId, response.StatusCode);
        if (response.IsSuccessStatusCode)
        {
            var req = await response.ParseResponseAsync<RequisitionV2>();
            if (req is not null)
            {
                return req;
            }
        }

        return null;
    }
    
    public async Task DeleteRequisition(Guid requisitionId)
    {
        _logger.LogInformation("Delete requisition for {RequisitionId}", requisitionId);
        if(await GetTokenCached() is not { } token)
        {
            return; // TODO: Add error handling
        }
        
        var endpoint = _options.Value.RequisitionEndpoint!;
        endpoint += requisitionId.ToString() + '/';
        var response = await _client.SendDeleteRequest(endpoint, token);
        
        _logger.LogInformation("Request to delete requisition {RequisitionId} returned with status code {StatusCode}", requisitionId, response.StatusCode);
        return; // TODO: Error response
    }

    public Task<AccountV2?> GetAccountMetadata(Guid accountId)
    {
        return _cache.GetOrAddAsync(_accountsPrefixCacheString + accountId.ToString(),
            () => GetAccountMetadataFromCacheOrApi(accountId));
    }
    
    private async Task<AccountV2?> GetAccountMetadataFromCacheOrApi(Guid accountId)
    {
        _logger.LogInformation("Get metadata for account {AccountId}", accountId.ToString());
        if(await GetTokenCached() is not { } token)
        {
            return null;
        }
        
        var endpoint = _options.Value.AccountEndpoint!;
        endpoint += accountId.ToString() + '/';
        var response = await _client.SendGetRequest(endpoint, token);
        
        _logger.LogInformation("Request to get account metadata from {AccountId} returned with status code {StatusCode}", accountId, response.StatusCode);
        if (response.IsSuccessStatusCode)
        {
            var account = await response.ParseResponseAsync<AccountV2>();
            if (account is not null)
            {
                return account;
            }
        }

        return null;
    }

    public async Task<ListTransactionsResponse?> GetAccountTransactions(Guid accountId, DateOnly? dateFrom = null, DateOnly? dateTo = null)
    {
        _logger.LogInformation("Get transactions for account {AccountId} from {DateFrom} to {DateTo}", accountId.ToString(), dateFrom, dateTo);
        if(await GetTokenCached() is not { } token)
        {
            return null;
        }
        
        var endpoint = _options.Value.AccountEndpoint!;
        endpoint += accountId.ToString() + "/transactions/";
        if (dateFrom is not null) // TODO: Error handling when only dateTo is set
        {
            endpoint += "?date_from=" + dateFrom.Value.ToString(NordigenFormatString);
            endpoint += dateTo is null ? string.Empty : "&date_to=" + dateTo.Value.ToString(NordigenFormatString);
        }
        
        var response = await _client.SendGetRequest(endpoint, token);
        
        _logger.LogInformation("Request to get transactions from {AccountId} returned with status code {StatusCode}", accountId, response.StatusCode);
        if (response.IsSuccessStatusCode)
        {
            var transactions = await response.ParseResponseAsync<ListTransactionsResponse>();
            if (transactions is not null)
            {
                return transactions;
            }
        }

        return null;
    }
}
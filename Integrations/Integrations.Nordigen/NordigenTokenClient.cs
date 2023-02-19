using System.Net.Http.Headers;
using System.Text;
using Integrations.Nordigen.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Integrations.Nordigen;

public record NewTokenResult(SpectacularJWTObtain? Token, bool Success = true, string? Error = null);
public record RefreshTokenResult(SpectacularJWTRefresh? Token, bool Success = true, string? Error = null);

public class NordigenTokenClient
{
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;
    
    private const string _newTokenEndpoint = "/new";
    private const string _refreshTokenEndpoint = "/refresh";

    public NordigenTokenClient(HttpClient client, IOptions<NordigenOptions> options)
    {
        _client = client;
        _options = options;

        client.BaseAddress = new Uri(options.Value.TokenEndpoint);
    }

    public async Task<NewTokenResult> GetNewToken()
    {
        JWTObtainPairRequest requestBody = new()
        {
            SecretId = _options.Value.ClientId,
            SecretKey = _options.Value.ClientSecret
        };

        var response = await SendRequest(requestBody, _refreshTokenEndpoint);
        var jwt = await ParseResponse<SpectacularJWTObtain>(response);
        if (jwt is not null)
        {
            new NewTokenResult(jwt);
        }
        
        return new NewTokenResult(null, false, "An error occured while requesting the token.");
    }

    public async Task<RefreshTokenResult> RefreshToken(string refreshToken)
    {
        JWTRefreshRequest requestBody = new()
        {
            Refresh = refreshToken
        };

        var response = await SendRequest(requestBody, _refreshTokenEndpoint);
        var jwt = await ParseResponse<SpectacularJWTRefresh>(response);
        if (jwt is not null)
        {
            new RefreshTokenResult(jwt);
        }
        
        return new RefreshTokenResult(null, false, "An error occured while refreshing the token.");
    }
    
    private async Task<TResponse?> ParseResponse<TResponse>(HttpResponseMessage response) where TResponse : class
    {
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        return null;
    }

    private Task<HttpResponseMessage> SendRequest<TRequest>(TRequest requestBody, string endpoint) 
        where TRequest : class
    {
        HttpRequestMessage request = new(HttpMethod.Post, _newTokenEndpoint);
        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        request.Content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json");
        
        return _client.SendAsync(request);
    }
}
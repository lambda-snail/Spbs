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

    public NordigenTokenClient(HttpClient client, IOptions<NordigenOptions> options)
    {
        _client = client;
        _options = options;

        client.BaseAddress = new Uri(options.Value.ServiceUrl);
    }

    public async Task<NewTokenResult> ObtainTokenAsync()
    {
        JWTObtainPairRequest requestBody = new(_options.Value.ClientId, _options.Value.ClientSecret);

        var response = await SendRequest(requestBody, _options.Value.NewTokenEndpoint);
        var jwt = await ParseResponse<SpectacularJWTObtain>(response);
        if (jwt is not null)
        {
            new NewTokenResult(jwt);
        }
        
        return new NewTokenResult(null, false, "An error occured while requesting the token.");
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken)
    {
        JWTRefreshRequest requestBody = new(refreshToken);

        var response = await SendRequest(requestBody, _options.Value.NewTokenEndpoint);
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
        Console.WriteLine(content);
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        return null;
    }

    private Task<HttpResponseMessage> SendRequest<TRequest>(TRequest requestBody, string endpoint) 
        where TRequest : class
    {
        HttpRequestMessage request = new(HttpMethod.Post, endpoint);
        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        request.Content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json");
        
        return _client.SendAsync(request);
    }
}
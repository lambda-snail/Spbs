using Integrations.Nordigen.Extensions;
using Integrations.Nordigen.Models;
using Microsoft.Extensions.Options;

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
        var response = await _client.SendPostRequest(requestBody, _options.Value.NewTokenEndpoint, null);
        var jwt = await response.ParseResponseAsync<SpectacularJWTObtain>();
        if (jwt is not null)
        {
            return new NewTokenResult(jwt);
        }
        
        return new NewTokenResult(null, false, "An error occured while requesting the token.");
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken)
    {
        JWTRefreshRequest requestBody = new(refreshToken);
        var response = await _client.SendPostRequest(requestBody, _options.Value.NewTokenEndpoint, null);
        var jwt = await response.ParseResponseAsync<SpectacularJWTRefresh>();
        if (jwt is not null)
        {
            return new RefreshTokenResult(jwt);
        }
        
        return new RefreshTokenResult(null, false, "An error occured while refreshing the token.");
    }
}
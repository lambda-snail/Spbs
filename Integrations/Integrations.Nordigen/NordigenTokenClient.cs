using AutoMapper;
using Integrations.Nordigen.Extensions;
using Integrations.Nordigen.Models;
using Microsoft.Extensions.Options;

namespace Integrations.Nordigen;

public record NewTokenResult(TokenPair? Token, bool Success = true, string? Error = null);
public record RefreshTokenResult(TokenRefreshResult? Token, bool Success = true, string? Error = null);

public class NordigenTokenClient
{
    private readonly IMapper _mapper; 
    private readonly HttpClient _client;
    private readonly IOptions<NordigenOptions> _options;

    public NordigenTokenClient(HttpClient client, IOptions<NordigenOptions> options, IMapper mapper)
    {
        _client = client;
        _options = options;
        _mapper = mapper;

        client.BaseAddress = new Uri(options.Value.ServiceUrl);
    }

    public async Task<NewTokenResult> ObtainTokenAsync()
    {
        JWTObtainPairRequest requestBody = new(_options.Value.ClientId, _options.Value.ClientSecret);
        var response = await _client.SendPostRequest(requestBody, _options.Value.NewTokenEndpoint, null);
        var jwtObtain = await response.ParseResponseAsync<SpectacularJWTObtain>();
        var jwt = _mapper.Map<TokenPair>(jwtObtain);
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
        var jwtRefresh = await response.ParseResponseAsync<SpectacularJWTRefresh>();
        var jwt = _mapper.Map<TokenRefreshResult>(jwtRefresh);
        if (jwt is not null)
        {
            return new RefreshTokenResult(jwt);
        }
        
        return new RefreshTokenResult(null, false, "An error occured while refreshing the token.");
    }
}
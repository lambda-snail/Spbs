namespace Integrations.Nordigen.Models;

public class TokenPair
{
    public TokenPair(string access, string refresh, int accessExpiresSeconds, int refreshExpiresSeconds)
    {
        UpdateAccessToken(access, accessExpiresSeconds);
        
        Refresh = refresh;
        RefreshExpires = DateTimeOffset.UtcNow.AddSeconds(refreshExpiresSeconds);
    }
    
    public string Access { get; set; }
    public DateTimeOffset AccessExpires { get; private set; }
    
    public string Refresh { get; private set; }
    public DateTimeOffset RefreshExpires { get; }

    public void UpdateAccessToken(string access, int accessExpiresSeconds)
    {
        UpdateAccessToken(access, DateTimeOffset.UtcNow.AddSeconds(accessExpiresSeconds));
    }
    
    public void UpdateAccessToken(string access, DateTimeOffset accessExpires)
    {
        Access = access;
        AccessExpires = accessExpires;
    }
    
    /// <summary>
    /// Returns true if the access token has (will have) expired on the given reference time.
    /// </summary>
    public bool HasAccessTokenExpiredAtTime(DateTimeOffset time) => time > AccessExpires;
    
    /// <summary>
    /// Returns true if the refresh token has (will have) expired on the given reference time.
    /// </summary>
    public bool HasRefreshTokenExpiredAtTime(DateTimeOffset time) => time > RefreshExpires;
}
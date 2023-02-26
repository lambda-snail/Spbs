namespace Integrations.Nordigen.Models;

public class TokenRefreshResult
{
    public string Access { get; set; }
    public DateTimeOffset AccessExpires { get; set; }
    
    public TokenRefreshResult(string access, int accessExpiresSeconds)
    {
        Access = access;
        AccessExpires = DateTime.UtcNow.AddSeconds(accessExpiresSeconds);
    }
}
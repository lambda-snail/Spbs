#nullable enable
using System.Security.Claims;

namespace Spbs.Ui.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is not null && claimsPrincipal.Identity.IsAuthenticated)
        {
            var userIdClaim = claimsPrincipal.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            if (userIdClaim is not null)
            {
                var userId = userIdClaim.Value;
                return Guid.Parse(userId);
            }
        }

        return null;
    }
}
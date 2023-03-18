using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.BankIntegration.Services;

public class RedirectLinkService : IRedirectLinkService
{
    private readonly NavigationManager _navigationManager;

    public RedirectLinkService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    /// <summary>
    /// Get the base url for the currently running application.
    /// </summary>
    public string GetBaseUrl()
    {
        return _navigationManager.BaseUri;
    }

    public string GetUrlForAccountListing()
    {
        return GetBaseUrl() + "account-links";
    }
}
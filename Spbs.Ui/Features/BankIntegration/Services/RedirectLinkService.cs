using System;
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
        return GetBaseUrl() + "accounts/links/";
    }

    public string GetUrlForImportExpenses(bool isInProgressPage = false)
    {
        return "/accounts/import-expenses" + (isInProgressPage ? "/in-progress" : string.Empty);
    }

    public string GetUrlForAccountListing(Guid linkId)
    {
        return GetUrlForAccountListing() + linkId;
    }

    public string GetUrlForNordigenRedirect()
    {
        return GetBaseUrl() + "accounts/redirect/";
    }
}
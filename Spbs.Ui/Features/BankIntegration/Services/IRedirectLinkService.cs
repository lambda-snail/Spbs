using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface IRedirectLinkService
{
    /// <summary>
    /// Get the base url for the currently running application.
    /// </summary>
    string GetBaseUrl();

    string GetUrlForAccountListing();
}
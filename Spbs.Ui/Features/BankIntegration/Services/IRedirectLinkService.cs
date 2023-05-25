using System;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface IRedirectLinkService
{
    /// <summary>
    /// Get the base url for the currently running application.
    /// </summary>
    string GetBaseUrl();

    string GetUrlForAccountListing();
    public string GetUrlForImportExpenses(bool isInProgressPage = false);
    string GetUrlForAccountListing(Guid linkId);

    string GetUrlForNordigenRedirect();
}
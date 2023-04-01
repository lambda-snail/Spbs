#nullable enable

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinksOverviewPage
{
    [Inject] private INordigenLinkReaderRepository _linkReader { get; set; }

    private ReadOnlyCollection<NordigenLink>? _userLinks;

    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        _userLinks = await _linkReader.GetLinksForUser(userId!.Value);
    }
}
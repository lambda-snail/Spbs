#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;
using Spbs.Ui.ComponentServices;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinksOverviewPage : ComponentBase
{
    [Inject, MaybeNull] private INordigenLinkReaderRepository _linkReader { get; set; }
    [Inject, MaybeNull] private INordigenLinkWriterRepository _linkWriter { get; set; }
    [Inject, MaybeNull] private INordigenApiClient _nordigenCLient { get; set; }
    [Inject, MaybeNull] private INotificationService _notificationService { get; set; }
    [Inject, MaybeNull] private IMapper _mapper { get; set; }
    [Inject, MaybeNull] private IRedirectLinkService _redirectService { get; set; }

    private ReadOnlyCollection<NordigenLink>? _userLinks = new(new List<NordigenLink>());
    private ReadOnlyCollection<Institution>? _institutions;
    private HashSet<NordigenLink> _selectedLinks = new();

    protected override async Task OnInitializedAsync()
    {
        await InitInstitutionList();
        await GetLinksForUser();
        StateHasChanged();
    }

    private async Task GetLinksForUser()
    {
        Guid? userId = await UserId();
        _userLinks = await _linkReader!.GetLinksForUser(userId!.Value);
    }

    /// <summary>
    /// Institutions are needed to populate certain properties of the link items.
    /// </summary>
    private async Task InitInstitutionList()
    {
        // TODO: Remove hard coded country
        var aspsps = await _nordigenCLient!.GetListOfInstitutionsAsync("SE");
        var institutions = _mapper!.Map<List<Institution>>(aspsps);

#if DEBUG
        institutions.Add(new Institution { Name = "Sandbox", Id = "SANDBOXFINANCE_SFIN0000", Bic = string.Empty });
#endif

        _institutions = new(institutions);
    }

    private async Task DeleteSelectedLinks()
    {
        if (_selectedLinks is not { Count: >0 })
        {
            return;
        }

        foreach (var link in _selectedLinks)
        {
            await _linkWriter!.DeleteLink(link);

            if (link is { NordigenId: not null })
            {
                _nordigenCLient?.DeleteRequisition(link.NordigenId.Value);
            }

            // TODO: Error handling
            _notificationService?.ShowToast("Link Deleted", $"Link to {link.InstitutionId} was successfully deleted!",
                NotificationLevel.Success);
        }

        // Slightly inefficient, but the user will probably not delete that many at a time so should be ok :)
        _userLinks = new(_userLinks!.Where(l => !_selectedLinks.Contains(l)).ToList());

        _selectedLinks.Clear();

        StateHasChanged();
    }

    private Task OnSelectedItemsChanged(HashSet<NordigenLink>? selection)
    {
        _selectedLinks = selection is not null && selection.Any() ? selection : new();
        return Task.CompletedTask;
    }
}
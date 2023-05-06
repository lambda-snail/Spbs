#nullable enable

using System;
using System.Collections.Generic;
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

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinksOverviewPage : SelectableListComponent<NordigenLink>
{
    [Inject, MaybeNull] private INordigenLinkReaderRepository _linkReader { get; set; }
    [Inject, MaybeNull] private INordigenLinkWriterRepository _linkWriter { get; set; }
    [Inject, MaybeNull] private INordigenApiClient _nordigenCLient { get; set; }
    [Inject, MaybeNull] private INotificationService _notificationService { get; set; }
    [Inject, MaybeNull] private IMapper _mapper { get; set; }
    
    protected override List<NordigenLink>? GetList() => _userLinks?.ToList();
    
    private ReadOnlyCollection<NordigenLink>? _userLinks;
    private ReadOnlyCollection<Institution>? _institutions;
    
    protected override async Task OnInitializedAsync()
    {
        await InitInstitutionList();
        await GetLinksForUser();
        StateHasChanged();
    }
    
    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
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

    private async Task DeleteSelectedLink()
    {
        int? selectedLink = GetSelected();
        if (_userLinks is null or { Count: 0 } || selectedLink is null)
        {
            return;
        }

        var link = _userLinks[selectedLink.Value];
        await _linkWriter!.Delete(link);

        var left = _userLinks.Take(selectedLink.Value).ToList();
        var right = _userLinks.Skip(selectedLink.Value + 1);
        
        left.AddRange(right as NordigenLink[] ?? right.ToArray());
        _userLinks = new(left);

        if (link is { NordigenId: not null })
        {
            _nordigenCLient?.DeleteRequisition(link.NordigenId.Value);            
        }
        
        // TODO: Error handling
        _notificationService?.ShowToast("Link Deleted", $"Link to {link.InstitutionId} was successfully deleted!", NotificationLevel.Success);
        StateHasChanged();
    }
}
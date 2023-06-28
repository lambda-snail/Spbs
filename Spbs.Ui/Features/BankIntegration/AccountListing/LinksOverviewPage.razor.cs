#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinksOverviewPage : ComponentBase
{
#pragma warning disable CS8618
    [Inject] private INordigenLinkReaderRepository _linkReader { get; set; }
    [Inject] private INordigenLinkWriterRepository _linkWriter { get; set; }
    [Inject] private INordigenApiClient _nordigenCLient { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    [Inject] private IRedirectLinkService _redirectService { get; set; }
    
    private MudDataGrid<NordigenLink> _grid;
#pragma warning restore CS8618
    
    private ReadOnlyCollection<NordigenLink>? _userLinks = new(new List<NordigenLink>());

    private Dictionary<string, Institution>? _institutions;
    private int _numSelectedLinks = 0;
    private bool _isDataLoaded = false;
    
    private static readonly int _numSkeletonsWhenLoading = 3;

    protected override async Task OnInitializedAsync()
    {
        await InitInstitutionList();
        await GetLinksForUser();

        _isDataLoaded = true;
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

        _institutions = institutions.ToDictionary(i => i.Id);
    }

    private async Task DeleteSelectedLinks()
    {
        HashSet<NordigenLink> selection = _grid.SelectedItems;
        if (selection is not { Count: >0 })
        {
            _snackbar.Add("No links selected", Severity.Error);
            return;
        }

        foreach (var link in selection)
        {
            await _linkWriter.DeleteLink(link);

            if (link is { NordigenId: not null })
            {
                _nordigenCLient?.DeleteRequisition(link.NordigenId.Value);
            }
        }
        
        _snackbar.Add("Link(s) deleted successfully", Severity.Success);

        // Slightly inefficient, but the user will probably not delete that many at a time so should be ok :)
        _userLinks = new(_userLinks!.Where(l => !selection.Contains(l)).ToList());

        selection.Clear();
        await _grid.ReloadServerData();
        StateHasChanged();
    }

    private string DeleteButtonTooltip()
    {
        if (_grid.Selection is null || _grid.Selection is { Count: 0 })
        {
            return "Delete links (none selected)";
        }

        if (_grid.Selection is { Count: 1 })
        {
            return "Delete link";
        }
        
        return "Delete links";
    }

    private void SelectedItemsChanged(HashSet<NordigenLink> selection)
    {
        _numSelectedLinks = selection.Count;
    }
}
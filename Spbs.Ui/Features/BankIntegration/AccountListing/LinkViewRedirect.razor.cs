#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinkViewRedirect
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        
        // Takes the 'ref' parameter from Nordigen and reroutes to a path parameter
        // Do not place in OnInitialized; see https://stackoverflow.com/questions/58076758/navigationerror-on-navigateto
        var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("ref", out var linkId))
        {
            var linkGuid = Guid.Parse(linkId!);
            await LoadAccountsFromNordigen(linkGuid);
            
            var url = _redirectLinkService.GetUrlForAccountListing();
            _navigationManager.NavigateTo(url + linkId);
        }
    }

    /// <summary>
    /// First time set up requires that we load any additional data from nordigen into the link in the database.
    /// Most notably the list of linked accounts.
    /// </summary>
    private async Task LoadAccountsFromNordigen(Guid linkId)
    {
        _userId = await UserId();        
        var internalLink = await _linkReaderRepository.GetLinkById(linkId, _userId!.Value);
        if (internalLink is null or { NordigenId: null })
        {
            return;
        }
        
        var dataFromNordigen = await _accountLinkService.GetLink(internalLink.NordigenId.Value);
        if (dataFromNordigen is not null)
        {
            _mapper.Map(dataFromNordigen, internalLink);
            await _accountLinkService.SaveLinkToDatabase(internalLink);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinkDetailsComponent : SelectableListComponent<Guid>
{
    [Parameter] public string RequisitionId { get; set; }

    [Inject] private INordigenApiClient _client { get; set; }
    [Inject] private INordigenLinkReaderRepository _linkReader { get; set; }

    private NordigenLink? _link;
    private List<AccountV2> _accounts = new();

    private TransactionsRequestParameters _transactionsRequestParameters = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Guid? userId = await UserId();
        ArgumentNullException.ThrowIfNull(userId, "UserId");
        
        Guid linkId = Guid.Parse(RequisitionId);
        _link = await _linkReader.GetLinkById(linkId, userId.Value);

        if (_link is { Accounts: not null and { Count: > 0 } })
        {
            _accounts = new(_link.Accounts.Count);
            foreach (var accountId in _link.Accounts)
            {
                var account = await _client.GetAccountMetadata(accountId);
                if (account is null)
                {
                    continue;
                }
                
                _accounts.Add(account);
                StateHasChanged();
            }
        }
        
        // if (_link is { Accounts: not null and { Count: > 0 } })
        // {
        //     var accountMetadataTasks = _link!.Accounts.Select(accountId => _client.GetAccountMetadata(accountId)).ToArray();
        //     var results = await Task.WhenAll(accountMetadataTasks);
        //     _accounts = results.Where(account => account is not null).Select(a => a).ToList();
        // }
    }

    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
    }

    protected override List<Guid>? GetList()
    {
        return _link?.Accounts;
    }

    private void HandleValidSubmit()
    {
        Console.WriteLine("VALID");
    }

    private void HandleInvalidSubmit()
    {
        Console.WriteLine("INVALID");
    }
}
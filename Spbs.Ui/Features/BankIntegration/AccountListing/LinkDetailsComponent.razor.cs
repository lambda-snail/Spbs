using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;
using Spbs.Ui.Features.BankIntegration.ImportExpenses;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

public class LoadAccountsParameters
{
    public bool IncludePending { get; set; }
    public bool IncludeAll { get; set; }
}

[AuthenticationTaskExtension]
public partial class LinkDetailsComponent : SelectableListComponent<Guid>
{
    [Parameter] public string RequisitionId { get; set; }

    [Inject] private INordigenApiClient _client { get; set; }
    [Inject] private INordigenLinkReaderRepository _linkReader { get; set; }
    [Inject] private INordigenAccountLinkService _accountService { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    
    private NordigenLink? _link;
    private List<AccountV2> _accounts = new();

    private TransactionsRequestParameters _transactionsRequestParameters = new();
    private List<ImportExpensesViewModel> _loadedTransactions = new();

    private LoadAccountsParameters _loadAccountsParameters = new();

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
    }

    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
    }

    protected override List<Guid>? GetList()
    {
        return _link?.Accounts;
    }

    private async Task HandleValidSubmit_LoadTransactionsFromNordigen()
    {
        var accountIndex = GetSelected();
        if (accountIndex is not null && accountIndex >= 0)
        {
            var accountId = _link?.Accounts[accountIndex.Value];
            var response = await _accountService.GetAccountTransactions(accountId!.Value, _transactionsRequestParameters);
            if (response is not null)
            {
                var completedTransactions = _mapper.Map<List<ImportExpensesViewModel>>(response.Transactions.Booked);
                var pendingTransactions = _mapper.Map<List<ImportExpensesViewModel>>(response.Transactions.Pending);

                foreach (var transaction in completedTransactions)
                {
                    transaction.IsPending = false;
                }
                
                foreach (var transaction in pendingTransactions)
                {
                    transaction.IsPending = true;
                }

                _loadedTransactions = new();
                _loadedTransactions.AddRange(completedTransactions);
                _loadedTransactions.AddRange(pendingTransactions);
            }
        }
    }

    private void HandleInvalidSubmit()
    {
        Console.WriteLine("INVALID");
    }
}
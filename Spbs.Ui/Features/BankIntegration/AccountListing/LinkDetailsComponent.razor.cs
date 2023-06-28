using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Integrations.Nordigen;
using Integrations.Nordigen.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;
using Spbs.Ui.Features.BankIntegration.ImportExpenses;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;
using Severity = MudBlazor.Severity;

namespace Spbs.Ui.Features.BankIntegration.AccountListing;

[AuthenticationTaskExtension]
public partial class LinkDetailsComponent : SelectableListComponent<Guid>
{
#pragma warning disable CS8618
    [Parameter] public string RequisitionId { get; set; }

    [Inject] private INordigenApiClient _client { get; set; }
    [Inject] private INordigenLinkReaderRepository _linkReader { get; set; }
    [Inject] private INordigenAccountLinkService _accountService { get; set; }
    [Inject] private IRedirectLinkService _redirectService { get; set; }
    [Inject] private ImportExpensesStateManager _importState { get; set; }
    [Inject] private NavigationManager _navigationManager { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    [Inject] private IValidator<TransactionsRequestParameters> _filterValidator { get; set; }

    private MudDataGrid<AccountV2> _grid;
#pragma warning restore CS8618

    private NordigenLink? _link;
    private List<AccountV2> _accounts = new();
    private HashSet<AccountV2> _selectedAccounts = new();

    private TransactionsRequestParameters _transactionsRequestParameters = new()
    {
        Range = new DateRange(new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1), DateTime.Today)
    };

    private List<ImportExpensesViewModel> _loadedTransactions = new();

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

    protected override List<Guid>? GetList()
    {
        return _link?.Accounts;
    }

    private async Task LoadTransactionsFromNordigen()
    {
        if (!IsFilterValid())
        {
            return;
        }

        var selectedAccount = _selectedAccounts.First();

        var accountId = selectedAccount.Id; //_link?.Accounts[accountIndex.Value];
        var response = await _accountService.GetAccountTransactions(accountId, _transactionsRequestParameters);
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

        _snackbar.Add("Transactions loaded successfully! Proceed to import to filter further before committing.");
    }

    private bool IsFilterValid()
    {
        var validationResult = _filterValidator.Validate(_transactionsRequestParameters);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _snackbar.Add(error.ErrorMessage, Severity.Error);
            }
        }

        return validationResult.IsValid;
    }

    private void ProceedToImportPage()
    {
        if (_loadedTransactions is { Count: > 0 })
        {
            _importState._expensesToConfigure = _loadedTransactions;
            string importUrl = _redirectService.GetUrlForImportExpenses();
            _navigationManager.NavigateTo(importUrl);
        }
    }

    private void OnSelectionChanged(HashSet<AccountV2> selection)
    {
        _selectedAccounts = selection;
    }

    private string GetLoadButtonTooltip()
    {
        return _selectedAccounts switch
        {
            { Count: 0 } => "Load transactions from the selected institution (no account selected)",
            { Count: 1 } => "Load transactions from the selected institution", 
            _ => "Load transactions from the selected institution (too many accounts selected)"
        };
    }

    private string GetImportButtonTooltip()
    {
        return _loadedTransactions switch
        {
            { Count: 0 } => "Import transactions to your account (no transactions loaded)",
            _ => "Proceed to import transactions to your account"
        };
    }
}
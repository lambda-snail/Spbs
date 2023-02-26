#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Spbs.Ui.Auth;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseListFilter
{
    public RecurrenceType? RecurrenceType { get; set; }
}

public partial class RecurringExpensesListComponent : SelectableListComponent<RecurringExpense>
{
    [Parameter] public Action<RecurringExpense> OnSelect { get; set; }
    
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }
    [Inject] public IRecurringExpenseReaderRepository RecurringExpenseReaderRepository { get; set; }

    private RecurringExpenseListFilter? _filter;
    private List<RecurringExpense>? _recurringExpenses;
    protected override List<RecurringExpense>? GetList() => _recurringExpenses;
    
    EditRecurringExpenseComponent _editRecurringExpensesDialog;
    
    private Dictionary<RecurrenceType, string> _billingTypeUIString = new ()
    {
        {RecurrenceType.Bill, "Bills" },
        {RecurrenceType.Invoice, "Invoices" },
        {RecurrenceType.Mortgage, "Mortgages" },
        {RecurrenceType.Subscription, "Subscriptions" },
    };

    protected override async Task OnInitializedAsync()
    {
        await FetchRecurringExpenses();
        await UserId();
    }

    public Task SetFilter(RecurringExpenseListFilter? filter)
    {
        _filter = filter;
        return FetchRecurringExpenses();
    }

    public Task ClearFitler()
    {
        return SetFilter(null);
    }

    private async Task FetchRecurringExpenses()
    {
        await UserId();
        if (_cachedUserId is null)
        {
            return; 
        }

        List<RecurringExpense>? recurringExpenses = null;
        if (_filter is not null && _filter.RecurrenceType is not null)
        {
            recurringExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesByUserId(_cachedUserId.Value, _filter.RecurrenceType.Value);
        }
        else
        {
            recurringExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesByUserId(_cachedUserId.Value);
        }

        _recurringExpenses = recurringExpenses;
        StateHasChanged();
    }

    private Guid? _cachedUserId = null;
    private async Task<Guid?> UserId()
    {
        if (_cachedUserId is not null)
        {
            return _cachedUserId;
        }
        
        var authState = await authenticationStateTask;
        var user = authState.User;

        _cachedUserId = user.GetUserId();
        return _cachedUserId;
    }

    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : String.Empty;
    }

    private string GetBillingTypeUIText()
    {
        if (_filter?.RecurrenceType != null)
        {
            return _billingTypeUIString[_filter.RecurrenceType.Value];
        }
     
        return "Recurring Expenses";
    }
    
    private void ToggleExpenseDialog()
    {
        RecurringExpense? e = null;
        int? selectedRow = GetSelected();
        if (selectedRow is not null && selectedRow >= 0 && selectedRow < _recurringExpenses?.Count)
        {
            e = _recurringExpenses?[selectedRow.Value];
        }
        
        _editRecurringExpensesDialog?.SetModalContent(e);
        _editRecurringExpensesDialog?.ShowModal();
    }

    private void SetFocusRecurrenceType(RecurrenceType? type)
    {
        SetFilter(new()
        {
            RecurrenceType = type
        });
    }

    private async Task ItemAddedOrUpdated()
    {
        await FetchRecurringExpenses();
        StateHasChanged();
    }
}
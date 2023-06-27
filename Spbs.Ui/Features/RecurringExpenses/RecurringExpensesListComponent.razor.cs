#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseListFilter
{
    public RecurrenceType? RecurrenceType { get; set; }
}

[AuthenticationTaskExtension()]
public partial class RecurringExpensesListComponent : SelectableListComponent<RecurringExpense>
{
    [Parameter] public Action<RecurringExpense> OnSelect { get; set; }
    
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

    public Task ClearFilter()
    {
        return SetFilter(null);
    }

    private async Task FetchRecurringExpenses()
    {
        await UserId();
        if (_userId is null)
        {
            return; 
        }

        List<RecurringExpense>? recurringExpenses = null;
        if (_filter is { RecurrenceType: not null })
        {
            recurringExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesByUserId(_userId.Value, _filter.RecurrenceType.Value);
        }
        else
        {
            recurringExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesByUserId(_userId.Value);
        }
        
        _recurringExpenses = recurringExpenses;
        StateHasChanged();
    }

    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
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

    private void SelectedItemsChanged(HashSet<RecurringExpense> obj)
    {
        
    }
}
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.RecurringExpenses;

[AuthenticationTaskExtension()]
public partial class RecurringExpensesListComponent : SelectableListComponent<RecurringExpense>
{
#pragma warning disable CS8618
    [Parameter] public Action<RecurringExpense> OnSelect { get; set; }
    
    [Inject] private IRecurringExpenseReaderRepository _recurringExpenseReaderRepository { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    
    private MudDataGrid<RecurringExpense> _grid;
    EditRecurringExpenseComponent _editRecurringExpensesDialog;
#pragma warning restore CS8618
    
    private List<RecurringExpense> _recurringExpenses = new();
    private int _numSelectedExpenses = 0;
    
    protected override List<RecurringExpense>? GetList() => _recurringExpenses;
    
    private RecurrenceType? _filterRecurrence = null;
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

    private async Task FetchRecurringExpenses()
    {
        await UserId();
        if (_userId is null)
        {
            return; 
        }

        _recurringExpenses = await _recurringExpenseReaderRepository.GetRecurringExpensesByUserId(_userId.Value);
        StateHasChanged();
    }

    private async Task ItemAddedOrUpdated(RecurringExpense expense)
    {
        _recurringExpenses.Add(expense);
        await _grid.ReloadServerData();
        StateHasChanged();
    }

    private void SelectedItemsChanged(HashSet<RecurringExpense> selection)
    {
        _numSelectedExpenses = selection.Count;
    }

    private void AddRecurringExpense()
    {
        RecurringExpense e = new();
        _editRecurringExpensesDialog.SetModalContent(e);
        _editRecurringExpensesDialog.ShowModal();
    }

    private void EditSelectedExpense()
    {
        var selection = _grid.Selection;
        if (selection is not { Count: 1 })
        {
            _snackbar.Add("Please select one item to edit", Severity.Error);
        }

        RecurringExpense e = selection.First(); 
        
        _editRecurringExpensesDialog.SetModalContent(e);
        _editRecurringExpensesDialog.ShowModal();
    }

    private async Task DeleteExpenses()
    {
        var selection = _grid.Selection;
        if (selection is { Count: 0 })
        {
            _snackbar.Add("Please select at least one item to delete", Severity.Error);
        }

        foreach (var expense in _recurringExpenses)
        {
            _recurringExpenses.Remove(expense);
            // delete from repo
        }
        
        await _grid.ReloadServerData();
    }
    
    private string DeleteButtonTooltip()
    {
        if (_grid.Selection is { Count: 0 })
        {
            return "Delete expense (none selected)";
        }

        return "Delete expense";
    }

    private string EditButtonTooltip()
    {
        return _grid.Selection switch
        {
            { Count: 0 } => "Edit expense (none selected)",
            { Count: >1 } => "Edit expense (too many selected)",
            _ => "Edit expense"
        };
    }
}
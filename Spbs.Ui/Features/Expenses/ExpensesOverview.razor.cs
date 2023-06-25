using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpensesOverview : ComponentBase
{
    private DateTime? _dateFilterYearMonth;
    private List<Expense> _cachedExpenses = new();

    private int _numSelectedExpenses = 0;
    
#pragma warning disable CS8618
    private EditExpenseComponent _editExpenseComponent;
    private MudDataGrid<Expense> _grid;
    [Inject] private IExpenseReaderRepository _expenseReader { get; set; }
    [Inject] private IExpenseWriterRepository _expenseWriter { get; set; }
    [Inject] private ISnackbar _snackBar { get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        var now = DateTime.UtcNow;
        _dateFilterYearMonth = new DateTime(now.Year, now.Month, 1);

        await ReloadData(_dateFilterYearMonth);
    }

    private async Task ReloadData(DateTime? date)
    {
        if (date is null)
        {
            _cachedExpenses.Clear();
            return;
        }
        
        Guid? userId = await UserId();
        _cachedExpenses = await _expenseReader.GetMonthlyExpensesForUser(userId!.Value, date.Value);
    }

    public string GetExpenseDetailsUrl(Expense e)
    {
        return $"expenses/{e.Id}";
    }

    private async void ExpenseItemAdded()
    {

        StateHasChanged();
    }
    
    private void ToggleExpenseDialog()
    {
        _editExpenseComponent?.SetModalContent(null);
        _editExpenseComponent?.ShowModal();
    }

    private void DeleteExpenses()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: 0 })
        {
            // We shouldn't get here, but just in case
            _snackBar.Add("No expenses selected", Severity.Warning);
        }

        foreach (var expense in selectedItems)
        {
            //_expenseWriter.DeleteAsync(expense);
            _cachedExpenses.Remove(expense);
        }

        _grid.ReloadServerData();
        
        _snackBar.Add($"Deleted {selectedItems.Count} {(selectedItems.Count > 1 ? "expenses" : "expense")}", Severity.Success);
    }

    private void SelectedItemsChanged(HashSet<Expense> selection)
    {
        _numSelectedExpenses = selection.Count;
    }
}

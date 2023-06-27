#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.RecurringExpenses;

[AuthenticationTaskExtension()]
public partial class RecurringExpenseDetails
{
    private Guid? _expenseId;
    [Parameter]
    public string? ExpenseId
    {
        set => _expenseId = (value is null) ? null : Guid.Parse(value);
    }
    
#pragma warning disable CS8618
    [Inject] private IRecurringExpenseReaderRepository _expenseReader { get; set; }
    [Inject] private IRecurringExpenseWriterRepository _expenseWriter { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }

    private MudDataGrid<RecurringExpenseHistoryItem> _grid;
    private int _numSelectedHistoryItems = 0;
#pragma warning restore CS8618
    
    private RecurringExpense? _expense;

    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        if (userId is null)
        {
            return;
        }

        _expense = (_expenseId is not null) ? await _expenseReader.GetByIdAsync(userId.Value, _expenseId.Value) : new();
        StateHasChanged();
    }

    private Task OnHistoryItemChanged(RecurringExpenseHistoryItem obj)
    {
        return _expenseWriter.UpdateExpenseAsync(_expense!);
    }

    private async Task AddExpenseItem()
    {
        var newItem = new RecurringExpenseHistoryItem { Id = Guid.NewGuid() };
        
        _expense!.PaymentHistory.Add(newItem);
        await _grid.ReloadServerData();
        await _grid.SetEditingItemAsync(newItem);
    }

    private async Task EditSelectedItem()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: not 1 })
        {
            _snackbar.Add("Too many items selected", Severity.Warning);
        }

        await _grid.SetEditingItemAsync(selectedItems.First());
    }

    private async Task DeleteExpenseItems()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: 0 })
        {
            _snackbar.Add("No items selected", Severity.Warning);
        }

        foreach (var item in selectedItems)
        {
            _expense!.PaymentHistory.Remove(item);
        }
        
        selectedItems.Clear();
        _numSelectedHistoryItems = 0;
        
        await _expenseWriter.UpdateExpenseAsync(_expense!);
        _snackbar.Add("History item(s) successfully deleted", Severity.Success);
    }

    private void OnSelectedItemsChanged(HashSet<RecurringExpenseHistoryItem> obj)
    {
        _numSelectedHistoryItems = _grid.SelectedItems.Count;
    }
    
    private string DeleteButtonTooltip()
    {
        if (_numSelectedHistoryItems == 0 )
        {
            return "Delete item (none selected)";
        }

        return "Delete item";
    }

    private string EditButtonTooltip()
    {
        return _numSelectedHistoryItems switch
        {
            0 => "Edit item (none selected)",
            >1 => "Edit item (too many selected)",
            _ => "Edit item"
        };
    }
}
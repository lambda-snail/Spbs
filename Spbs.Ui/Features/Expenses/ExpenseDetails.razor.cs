using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpenseDetails : ComponentBase
{
    [Parameter] public string ExpenseId { get; set; }
    private Expense? _expense = null;
    
    private int _numSelectedExpenseItems = 0;
    
#pragma warning disable CS8618
    [Inject] private IExpenseReaderRepository _expenseReaderRepository { get; set; }
    [Inject] private IExpenseWriterRepository _expenseWriterRepository { get; set; }
    [Inject] private IJSRuntime _jsRuntime { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    
    [Inject] ISnackbar _snackbar { get; set; }
    
    private EditExpenseComponent _editExpenseComponent;
    private MudDataGrid<ExpenseItem> _grid;
#pragma warning restore CS8618
    
    protected override void OnInitialized()
    {
        FetchExpense();
    }

    private async void FetchExpense()
    {
        Guid id = Guid.Parse(ExpenseId);
        Guid? userId = await UserId();
        _expense = await _expenseReaderRepository.GetUserExpenseById(id, userId!.Value);
        StateHasChanged();
    }

    private async Task SaveExpense(string successMessage = "")
    {
        if (_expense is not null)
        {
            await _expenseWriterRepository.UpdateExpenseAsync(_expense);            
        }

        _snackbar.Add(string.IsNullOrWhiteSpace(successMessage) ? "Changes saved successfully!" : successMessage, Severity.Success);
        StateHasChanged();
    }

    private async Task AddTagList()
    {
        string tagList = await _jsRuntime.InvokeAsync<string>("prompt", "Add a list of tags separated by space");
        if (string.IsNullOrWhiteSpace(tagList))
        {
            return;
        }

        _expense!.Tags ??= string.Empty;

        _expense!.Tags += " " + tagList;
        await SaveExpense();
    }

    private void ExpenseUpdated()
    {
        FetchExpense();
    }
    
    private void ToggleEditMode()
    {
        _editExpenseComponent?.SetModalContent(_expense);
        _editExpenseComponent?.ShowModal();
    }

    private async Task AddExpenseItem()
    {
        var newItem = new ExpenseItem { Id = Guid.NewGuid() };
        
        _expense!.Items.Add(newItem);
        await _grid.ReloadServerData();
        await _grid.SetEditingItemAsync(newItem);
    }

    private Task OnExpenseItemChanged()
    {
        return SaveExpense();
    }
    
    private async Task EditSelectedItem()
    {
        var selectedItems = _grid.SelectedItems;
        if (selectedItems is { Count: not 1 })
        {
            _snackbar.Add("Too many items selected", Severity.Warning);
        }

        await  _grid.SetEditingItemAsync(selectedItems.First());
    }
    
    private void OnSelectedItemsChanged(HashSet<ExpenseItem> selection)
    {
        _numSelectedExpenseItems = selection.Count;
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
            _expense!.Items.Remove(item);
        }

        int numDeleted = selectedItems.Count;
        selectedItems.Clear();
        _numSelectedExpenseItems = 0;

        await SaveExpense("Deleted " + numDeleted + (numDeleted > 1 ? "items" : "item"));
    }
}
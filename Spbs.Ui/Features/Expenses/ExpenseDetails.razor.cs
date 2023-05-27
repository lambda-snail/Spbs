using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Auth;
using Spbs.Ui.Features.Users;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpenseDetails : ComponentBase
{
    [Parameter] public string ExpenseId { get; set; }

    [Inject] public IExpenseReaderRepository ExpenseReaderRepository { get; set; }
    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public IMapper Mapper { get; set; }

    private Expense? _expense = null;
    private EditExpenseComponent _editExpenseComponent;
    private EditExpenseItemComponent _editExpenseItemComponent;

    protected override void OnInitialized()
    {
        FetchExpense();
    }

    private async void FetchExpense()
    {
        Guid id = Guid.Parse(ExpenseId);
        Guid? userId = await UserId();
        _expense = await ExpenseReaderRepository.GetUserExpenseById(id, userId!.Value);
        StateHasChanged();
    }

    private async Task SaveExpense()
    {
        if (_expense is null)
        {
            return;
        }

        await ExpenseWriterRepository.UpdateExpenseAsync(_expense!);
        StateHasChanged();
    }

    private void ToggleEditMode()
    {
        _editExpenseComponent?.SetModalContent(_expense);
        _editExpenseComponent?.ShowModal();
    }

    private void ExpenseUpdated()
    {
        FetchExpense();
    }

    private async Task AddTagList()
    {
        string tagList = await JsRuntime.InvokeAsync<string>("prompt", "Add a list of tags separated by space");
        if (tagList is null || tagList == string.Empty)
        {
            return;
        }

        _expense!.Tags ??= string.Empty;

        _expense!.Tags += " " + tagList;
        await SaveExpense();
    }

    private async Task ExpenseItemUpdated(ExpenseItem? item)
    {
        if (item is null)
        {
            return;
        }

        if (_selectedRow >= 0 && _expense?.Items[_selectedRow.Value].Id == item.Id)
        {
            Mapper.Map((ExpenseItem)item, _expense.Items[_selectedRow.Value]);
        }
        else
        {
            _expense?.Items?.Add(item);
        }
        
        await SaveExpense();
        //FetchExpense();
    }
    
    #region ExpenseItems

    private void EditOrCreateExpenseItem(ExpenseItem item)
    {
        _editExpenseItemComponent?.SetModalContent(item);
        _editExpenseItemComponent?.ShowModal();
    }
    
    private void ToggleEditItemsMode()
    {
        if (_selectedRow is not null && _selectedRow >= 0)
        {
            ExpenseItem selected = _expense.Items[_selectedRow.Value];
            _editExpenseItemComponent?.SetModalContent(selected);
        }
        else
        {
            _editExpenseItemComponent?.SetModalContent(null);            
        }
        
        _editExpenseItemComponent?.ShowModal();
    }
    
    #endregion
    
    #region Selection
    
    private int? _selectedRow = null;
    private string GetRowClass(int i)
    {
        return _selectedRow == i ? "bg-secondary text-white" : String.Empty;
    }
    
    private void SetSelected(int i)
    {
        if (i >= 0 && i < _expense?.Items.Count)
        {
            _selectedRow = i == _selectedRow ? null : i;
            StateHasChanged();
        }
        else
        {
            if (_selectedRow is not null)
            {
                _selectedRow = null;
                StateHasChanged();
            }
        }
    }
    
    #endregion
}
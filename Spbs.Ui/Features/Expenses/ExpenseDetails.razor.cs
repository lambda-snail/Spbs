using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.Expenses;

public partial class ExpenseDetails : ComponentBase
{
    [Parameter] public string ExpenseId { get; set; }

    [Inject] public IExpenseReaderRepository ExpenseReaderRepository { get; set; }
    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public IMapper Mapper { get; set; }
    
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

    private Expense? _expense = null;
    private EditExpenseComponent _editExpenseComponent;
    private EditExpenseItemComponent _editExpenseItemComponent;

    private Guid? _userId = null;
    
    protected override void OnInitialized()
    {
        FetchExpense();
    }

    private async Task<bool> LoadUserIdCached()
    {
        if (_userId is null)
        {
            var userId = await UserId();
            _userId = userId;
        }
        
        return _userId is not null;
    }
    
    private async void FetchExpense()
    {
        if (!await LoadUserIdCached())
        {
            return;
        }

        Guid id = Guid.Parse(ExpenseId);
        _expense = await ExpenseReaderRepository.GetUserExpenseById(_userId!.Value, id);
        StateHasChanged();
    }

    private async Task SaveExpense()
    {
        if (!await LoadUserIdCached() || _expense is null)
        {
            return;
        }

        await ExpenseWriterRepository.UpdateExpenseAsync(_expense!);
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
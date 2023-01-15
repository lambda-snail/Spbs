using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseFilter 
{
    public DateTime? FromDate { get; set; }
    public bool FromDateMonthOnly { get; set; } = false;
}

public partial class ExpensesOverview : ComponentBase
{
    private List<Expense>? _expenses = null;
    private int? _selectedRow = null;
    private bool _displayFilter = false;
    
    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }
    
    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; }
    
    private ExpenseFilter _expenseFilter { get; set; } = new ExpenseFilter();
    
    private EditExpenseComponent _editExpenseComponent;
    
    public ExpensesOverview()
    {
        
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchExpenses();
        await UserId();
    }

    /// <summary>
    /// Determine which row is the selected one. If the selected row is clicked twice, it will be deselected.
    /// </summary>
    /// <param name="i"></param>
    private void SetSelected(int i)
    {
        if (i >= 0 && i < _expenses?.Count)
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

    private async Task FetchExpenses()
    {
        var authState = await authenticationStateTask;
        var user = authState.User;
        
        Guid? userId = user.GetUserId();
        if (userId is null)
        {
            return;
        }
        
        _expenses ??= new();
        // _expenses = await ExpenseRepository.GetSingleExpensesByUserAndMonth(userId.Value,
        //     new DateTime(ExpenseFilter.Year, ExpenseFilter.Month, 2));
        if (_expenseFilter?.FromDate is not null)
        {
            _expenses = await ExpenseRepository.GetSingleExpensesByUserAndMonth(userId.Value, _expenseFilter.FromDate.Value);
        }
        else
        {
            _expenses = await ExpenseRepository.GetSingleExpensesByUser(userId.Value);
        }
    }

    public string GetExpenseDetailsUrl(Expense e)
    {
        return $"expenses/{e.Id}";
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

    private async void ExpenseItemAdded()
    {
        await FetchExpenses();
        StateHasChanged();
    }
    
    private void ToggleExpenseDialog()
    {
        Expense? e = null;
        if (_selectedRow is not null && _selectedRow >= 0 && _selectedRow < _expenses?.Count)
        {
            e = _expenses?[_selectedRow.Value];
        }
        
        _editExpenseComponent?.SetModalContent(e);
        _editExpenseComponent?.ShowModal();
    }

    private string GetRowClass(int i)
    {
        return _selectedRow == i ? "bg-secondary text-white" : String.Empty;
    }

    private async void ApplyFilter()
    {
        await FetchExpenses();
        StateHasChanged();
    }
}

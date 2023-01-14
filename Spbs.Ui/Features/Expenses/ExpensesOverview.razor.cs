using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseFilter 
{
    public int Month { get; set; }
    public int Year { get; set; }
}

public partial class ExpensesOverview : ComponentBase
{
    private List<Expense>? _expenses = null;

    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }
    
    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; }
    
    public ExpenseFilter ExpenseFilter { get; set; } = new ExpenseFilter { Month = 12, Year = 2022 };
    
    private EditExpenseComponent _editExpenseComponent;
    
    public ExpensesOverview()
    {
        
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchExpenses();
        await UserId();
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
        _expenses = await ExpenseRepository.GetSingleExpensesByUser(userId.Value);
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
    
    private void ToggleAddExpenseMode()
    {
        _editExpenseComponent?.SetModalContent();
        _editExpenseComponent?.ShowModal();
    }
}

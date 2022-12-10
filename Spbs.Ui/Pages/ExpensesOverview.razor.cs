using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using Spbs.Ui.Auth;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Pages;

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
    
    public ExpensesOverview()
    {
        
    }

    protected override void OnInitialized()
    {
        FetchExpenses();
    }

    private async void FetchExpenses()
    {
        var authState = await authenticationStateTask;
        var user = authState.User;
        
        Guid? userId = user.GetUserId();
        if (userId is null)
        {
            return;
        }
        
        _expenses ??= new();
        _expenses = await ExpenseRepository.GetSingleExpensesByUserAndMonth(userId.Value,
            new DateTime(ExpenseFilter.Year, ExpenseFilter.Month, 2));
    }
}
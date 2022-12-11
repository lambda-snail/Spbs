using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.Expenses;

public partial class ExpenseDetails : ComponentBase
{
    [Parameter]
    public string ExpenseId { get; set; }
    
    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }
    
    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; }

    private Expense? _expense = null;

    protected override void OnInitialized()
    {
        FetchExpense();
    }
    
    private async void FetchExpense()
    {
        var authState = await authenticationStateTask;
        var user = authState.User;
        
        Guid? userId = user.GetUserId();
        if (userId is null)
        {
            return;
        }

        Guid id = Guid.Parse(ExpenseId);
        _expense = await ExpenseRepository.GetUserExpenseById(userId.Value, id);
    }
}
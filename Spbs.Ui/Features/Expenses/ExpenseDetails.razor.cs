using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.Expenses;

public partial class ExpenseDetails : ComponentBase
{
    [Parameter] public string ExpenseId { get; set; }

    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }

    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

    private Expense? _expense = null;
    private EditExpenseComponent _editExpenseComponent;

    protected override void OnInitialized()
    {
        FetchExpense();
    }

    private async void FetchExpense()
    {
        var userId = await UserId();
        if (userId is null)
        {
            return;
        }

        Guid id = Guid.Parse(ExpenseId);
        _expense = await ExpenseRepository.GetUserExpenseById(userId.Value, id);
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

    private void ExpenseItemUpdated()
    {
        FetchExpense();
    }
}
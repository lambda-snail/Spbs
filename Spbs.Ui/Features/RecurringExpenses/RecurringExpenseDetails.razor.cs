#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class RecurringExpenseDetails
{
    private Guid? _expenseId;
    [Parameter]
    public string? ExpenseId
    {
        set => _expenseId = (value is null) ? null : Guid.Parse(value);
    }
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }
    [Inject] public IRecurringExpenseReaderRepository _expenseReaderRepository { get; set; }

    private RecurringExpense? _expense;

    protected override async Task OnInitializedAsync()
    {
        _expense = (_expenseId is not null) ? await _expenseReaderRepository.GetByIdAsync(_expenseId.Value) : new();
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
}
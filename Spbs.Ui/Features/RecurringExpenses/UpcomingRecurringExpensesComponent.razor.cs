using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class UpcomingRecurringExpensesComponent
{
    [Inject] public IRecurringExpenseReaderRepository RecurringExpenseReaderRepository { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

    private List<RecurringExpense> _upcomingExpenses = new();
    
    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        if (userId is null) { return; }

        int today = DateTime.Now.Day;
        _upcomingExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesAfterDayOfMonth(userId.Value, today);
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
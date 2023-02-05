using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class RecurringExpensesListComponent
{
    [Parameter] public RecurrenceType? TypeFilter { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }
    
    [Inject] public IRecurringExpenseReaderRepository RecurringExpenseReaderRepository { get; set; }

    private List<RecurringExpense>? _recurringExpenses;
    
    private Dictionary<RecurrenceType, string> _billingTypeUIString = new () { {RecurrenceType.Subscription, "Subscription" } };

    protected override async Task OnInitializedAsync()
    {
        await FetchRecurringExpenses();
        await UserId();
    }

    private async Task FetchRecurringExpenses()
    {
        await UserId();
        if (_cachedUserId is null)
        {
            return; 
        }

        List<RecurringExpense>? recurringExpenses = null;
        if (TypeFilter is null)
        {
            recurringExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesByUserId(_cachedUserId.Value);
        }
        else
        {
            recurringExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesByUserId(_cachedUserId.Value, TypeFilter.Value);   
        }

        _recurringExpenses = recurringExpenses;
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
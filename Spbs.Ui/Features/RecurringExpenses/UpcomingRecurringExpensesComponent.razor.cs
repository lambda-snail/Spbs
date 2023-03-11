using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.RecurringExpenses;

[AuthenticationTaskExtension()]
public partial class UpcomingRecurringExpensesComponent
{
    [Inject] public IRecurringExpenseReaderRepository RecurringExpenseReaderRepository { get; set; }

    private List<RecurringExpense> _upcomingExpenses = new();
    
    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        if (userId is null) { return; }

        int today = DateTime.Now.Day;
        _upcomingExpenses = await RecurringExpenseReaderRepository.GetRecurringExpensesAfterDayOfMonth(userId.Value, today);
    }
}
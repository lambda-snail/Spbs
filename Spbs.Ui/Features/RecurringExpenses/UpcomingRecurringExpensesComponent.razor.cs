using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Utilities;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Features.RecurringExpenses;

[AuthenticationTaskExtension()]
public partial class UpcomingRecurringExpensesComponent
{
#pragma warning disable CS8618
    [Inject] private IRecurringExpenseReaderRepository _recurringExpenseReaderRepository { get; set; }
    [Inject] private IDateTimeProvider _dateTime { get; set; }
#pragma warning restore CS8618

    private List<RecurringExpense> _upcomingExpenses = new();
    
    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        if (userId is null) { return; }

        var today = _dateTime.Now();
        _upcomingExpenses = await _recurringExpenseReaderRepository.GetRecurringExpensesAfterDay(userId.Value, today.Day);
    }
}
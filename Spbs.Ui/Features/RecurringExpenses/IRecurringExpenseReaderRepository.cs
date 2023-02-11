using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spbs.Shared.Data;

namespace Spbs.Ui.Features.RecurringExpenses;

public interface IRecurringExpenseReaderRepository : IReaderRepositoryBase<RecurringExpense>
{
    Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid OwningUserId);
    Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid OwningUserId, RecurrenceType type);
    Task<List<RecurringExpense>> GetRecurringExpensesAfterDayOfMonth(Guid userId, int day);
}
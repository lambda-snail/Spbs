using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spbs.Ui.Features.RecurringExpenses;

public interface IRecurringExpenseReaderRepository
{
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid OwningUserId);
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid OwningUserId, RecurrenceType type);
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spbs.Shared.Data;

namespace Spbs.Ui.Features.RecurringExpenses;

public interface IRecurringExpenseReaderRepository : IReaderRepositoryBase<RecurringExpense>
{
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid OwningUserId);
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid OwningUserId, RecurrenceType type);
}
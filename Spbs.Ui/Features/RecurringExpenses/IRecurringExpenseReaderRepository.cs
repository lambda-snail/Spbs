using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spbs.Shared.Data;

namespace Spbs.Ui.Features.RecurringExpenses;

public interface IRecurringExpenseReaderRepository
{
    Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId);
    Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId, RecurrenceType type);
    Task<List<RecurringExpense>> GetRecurringExpensesAfterDay(Guid userId, int day);
    Task<RecurringExpense?> GetByIdAsync(Guid userId, Guid expenseId);
}
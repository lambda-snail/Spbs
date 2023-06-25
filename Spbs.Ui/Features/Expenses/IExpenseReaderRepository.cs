using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spbs.Ui.Features.Expenses;

public interface IExpenseReaderRepository
{
    Task<List<Expense>> GetSingleExpensesByUser(Guid userId, int take = 25, int skip = 0);
    
    Task<Expense?> GetUserExpenseById(Guid expenseId, Guid userId);

    /// <summary>
    /// Get expenses for the given user from the specified month.
    /// </summary>
    Task<List<Expense>> GetAllExpensesByUserFromMonth(Guid userId, DateTime date);
    Task<List<Expense>> GetSingleExpensesByUserFromMonth(Guid userId, DateTime date, int take = 25, int skip = 0);
}
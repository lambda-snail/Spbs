using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.Expenses;

public interface IExpenseReaderRepository
{
    Task<List<Expense>> GetSingleExpensesByUser(Guid userId, int take = 50, int skip = 0);
    
    Task<Expense?> GetUserExpenseById(Guid userId, Guid expenseId);

    Task<List<Expense>> GetSingleExpensesByUserAndMonth(Guid userId, DateTime monthYear, int take = 50, int skip = 0);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spbs.Shared.Data;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseReaderRepository : ReaderRepositoryBase<Expense, ExpensesDbContext>, IExpenseReaderRepository
{
    public ExpenseReaderRepository(IDbContextFactory<ExpensesDbContext> context) : base(context)
    {
        
    }

    public async Task<List<Expense>> GetSingleExpensesByUser(Guid userId, int take = 50, int skip = 0)
    {
        return await _db.Expenses.Where(exp =>
                exp.OwningUserId == userId)
            .OrderByDescending(e => e.Date)
            .Take(take)
            .Skip(skip)
            .ToListAsync();
    }
    
    public Task<Expense?> GetUserExpenseById(Guid userId, Guid expenseId)
    {
        return _db.Expenses.Where(exp =>
                exp.OwningUserId == userId && exp.Id == expenseId)
            .Include(exp => exp.Items)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Expense>> GetSingleExpensesByUserAndMonth(Guid userId, DateTime monthYear,
        int take = 50, int skip = 0)
    {
        DateTime firstDay = new DateTime(monthYear.Year, monthYear.Month, 1);
        DateTime lastDay = new DateTime(monthYear.Year, monthYear.Month, DateTime.DaysInMonth(monthYear.Year, monthYear.Month));

        return _db.Expenses
            .Where(exp => exp.OwningUserId == userId && exp.Date > firstDay && exp.Date < lastDay)
            .OrderByDescending(exp => exp.Date)
            //.Take(take)
            //.Skip(skip)
            .Include(exp => exp.Items)
            .ToList();
    }
}
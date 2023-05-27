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
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Expenses.Where(exp =>
                exp.OwningUserId == userId)
            .OrderByDescending(e => e.Date)
            .Take(take)
            .Skip(skip)
            .ToListAsync();
    }
    
    public async Task<Expense?> GetUserExpenseById(Guid userId, Guid expenseId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Expenses.Where(exp =>
                exp.OwningUserId == userId && exp.Id == expenseId)
            .Include(exp => exp.Items)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Expense>> GetSingleExpensesByUserAndMonth(
        Guid userId, DateTime date, int take = 50, int skip = 0)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Expenses
            .Where(exp => exp.OwningUserId == userId && exp.Date >= date)
            .OrderByDescending(exp => exp.Date)
            .Skip(skip)
            .Take(take)
            .Include(exp => exp.Items)
            .ToListAsync();
    }
}
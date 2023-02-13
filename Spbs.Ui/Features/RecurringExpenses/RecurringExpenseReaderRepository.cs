using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spbs.Shared.Data;
using Spbs.Ui.Data;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseReaderRepository : ReaderRepositoryBase<RecurringExpense, RecurringExpensesDbContext>, IRecurringExpenseReaderRepository
{
    public RecurringExpenseReaderRepository(IDbContextFactory<RecurringExpensesDbContext> context) : base(context) {}
    
    public async Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId)
            .Include(re => re.PaymentHistory)
            .ToListAsync();
    }
    
    public async Task<List<RecurringExpense>> GetRecurringExpensesAfterDayOfMonth(Guid userId, int day)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId && rexp.BillingDate.Day >= day)
            .OrderByDescending(rexp => rexp.BillingDate)
            .ToListAsync();
    }
    
    public async Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId, RecurrenceType type)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId && rexp.RecurrenceType == type)
            .Include(re => re.PaymentHistory)
            .ToListAsync();
    }

    public override async Task<RecurringExpense?> GetByIdAsync(Guid id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.RecurringExpenses.Where(rexp => rexp.Id == id)
            .Include(rexp => rexp.PaymentHistory)
            .FirstOrDefaultAsync();
    }
}
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
    public RecurringExpenseReaderRepository(RecurringExpensesDbContext context) : base(context) {}
    
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId)
    {
        return _db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId)
            .Include(re => re.PaymentHistory)
            .ToListAsync();
    }
    
    public Task<List<RecurringExpense>> GetRecurringExpensesAfterDayOfMonth(Guid userId, int day)
    {
        return _db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId && rexp.BillingDate.Day >= day)
            .ToListAsync();
    }
    
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId, RecurrenceType type)
    {
        return _db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId && rexp.RecurrenceType == type)
            .Include(re => re.PaymentHistory)
            .ToListAsync();
    }

    public override Task<RecurringExpense?> GetByIdAsync(Guid id)
    {
        return _db.RecurringExpenses.Where(rexp => rexp.Id == id)
            .Include(rexp => rexp.PaymentHistory)
            .FirstOrDefaultAsync();
    }
}
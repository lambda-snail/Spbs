using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spbs.Shared.Data;
using Spbs.Ui.Data;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseReaderRepository : ReaderRepositoryBase<Expense, RecurringExpensesDbContext>, IRecurringExpenseReaderRepository
{
    public RecurringExpenseReaderRepository(RecurringExpensesDbContext context) : base(context) {}
    
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId)
    {
        return _db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId)
            .ToListAsync();
    }
    
    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId, BillingType type)
    {
        return _db.RecurringExpenses.Where(rexp =>
                rexp.OwningUserId == userId && rexp.BillingType == type)
            .ToListAsync();
    }
}
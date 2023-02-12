using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Utilities;
using Spbs.Shared.Data;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseWriterRepository : WriterRepositoryBase<RecurringExpense, RecurringExpensesDbContext>, IRecurringExpenseWriterRepository
{
    private readonly IDateTimeProvider _dateTime;
    public RecurringExpenseWriterRepository(IDateTimeProvider dateTime, IDbContextFactory<RecurringExpensesDbContext> contextFactory) : base(contextFactory)
    {
        _dateTime = dateTime;
    }

    public Task<RecurringExpense> InsertExpenseAsync(RecurringExpense expense)
    {
        DateTime now = _dateTime.Now();
        UpdateAuditColumns(expense, now);
        SetOnCreateAuditColumns(expense, now);
        return InsertAsync(expense);
    }
    
    public Task UpdateExpenseAsync(RecurringExpense expense)
    {
        UpdateAuditColumns(expense, _dateTime.Now());
        return UpdateAsync(expense);
    }

    private void UpdateAuditColumns(RecurringExpense expense, DateTime modifiedOn)
    {
        expense.ModifiedOn = modifiedOn;
    }

    private void SetOnCreateAuditColumns(RecurringExpense expense, DateTime createdOn)
    {
        expense.CreatedOn = createdOn;
    }
}
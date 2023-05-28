using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Utilities;
using Spbs.Shared.Data;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseWriter : WriterRepositoryBase<Expense, ExpensesDbContext>, IExpenseWriterRepository
{
    private readonly IDateTimeProvider _dateTime;

    public ExpenseWriter(IDateTimeProvider dateTime, IDbContextFactory<ExpensesDbContext> contextFactory) :
        base(contextFactory)
    {
        _dateTime = dateTime;
    }

    public Task<Expense> InsertExpenseAsync(Expense expense)
    {
        DateTime now = _dateTime.Now();
        UpdateAuditColumns(expense, now);
        SetOnCreateAuditColumns(expense, now);
        return InsertAsync(expense);
    }
    
    public Task UpdateExpenseAsync(Expense expense)
    {
        UpdateAuditColumns(expense, _dateTime.Now());
        return UpdateAsync(expense);
    }

    private void UpdateAuditColumns(Expense expense, DateTime modifiedOn)
    {
        expense.ModifiedOn = modifiedOn;
    }

    private void SetOnCreateAuditColumns(Expense expense, DateTime createdOn)
    {
        expense.CreatedOn = createdOn;
    }
}
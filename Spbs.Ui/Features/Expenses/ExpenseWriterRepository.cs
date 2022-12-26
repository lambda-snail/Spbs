using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseWriterRepository : WriterRepositoryBase<Expense, ExpensesDbContext>, IExpenseWriterRepository
{
    private readonly IMapper _mapper;

    public ExpenseWriterRepository(IMapper mapper, ExpensesDbContext context) : base(context)
    {
        _mapper = mapper;
    }

    public Task<Expense> InsertAsync(EditExpenseViewModel editExpense)
    {
        var expense = _mapper.Map<Expense>(editExpense);
        
        DateTime now = DateTime.Now;
        UpdateAuditColumns(expense, now); // TODO Add Datetime provider or similar
        SetOnCreateAuditColumns(expense, now);
        return InsertAsync(expense);
    }
    
    public Task UpdateAsync(EditExpenseViewModel editExpense)
    {
        var expense = _mapper.Map<Expense>(editExpense);
        
        UpdateAuditColumns(expense, DateTime.Now);
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
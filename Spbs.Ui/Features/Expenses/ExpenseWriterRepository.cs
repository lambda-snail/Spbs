using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseWriterRepository : WriterRepositoryBase<Expense, ExpensesDbContext>, IExpenseWriterRepository
{
    public ExpenseWriterRepository(IDbContextFactory<ExpensesDbContext> factory) : base(factory) { }

    public override Task<Expense> InsertAsync(Expense row)
    {
        base.InsertAsync(row);
    }
}
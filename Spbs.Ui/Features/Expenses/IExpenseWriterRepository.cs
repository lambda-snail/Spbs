using System.Threading.Tasks;
using Spbs.Ui.Data;

namespace Spbs.Ui.Features.Expenses;

public interface IExpenseWriterRepository
{
    public Task<Expense> InsertAsync(EditExpenseViewModel editExpense);
    public Task UpdateAsync(EditExpenseViewModel editExpense);
}
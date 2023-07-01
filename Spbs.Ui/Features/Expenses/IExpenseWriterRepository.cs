using System.Threading.Tasks;

namespace Spbs.Ui.Features.Expenses;

public interface IExpenseWriterRepository
{
    public Task<Expense?> InsertExpenseAsync(Expense expense);
    public Task UpdateExpenseAsync(Expense expense);
}
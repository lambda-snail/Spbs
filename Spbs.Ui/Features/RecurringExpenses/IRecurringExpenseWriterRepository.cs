using System.Threading.Tasks;
using Spbs.Shared.Data;

namespace Spbs.Ui.Features.RecurringExpenses;

public interface IRecurringExpenseWriterRepository : IWriterRepositoryBase<RecurringExpense>
{
    Task<RecurringExpense> InsertExpenseAsync(RecurringExpense expense);
    Task UpdateExpenseAsync(RecurringExpense expense);
}
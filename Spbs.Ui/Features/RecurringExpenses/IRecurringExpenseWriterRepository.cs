using System.Threading.Tasks;
using Spbs.Shared.Data;

namespace Spbs.Ui.Features.RecurringExpenses;

public interface IRecurringExpenseWriterRepository
{
     Task<RecurringExpense?> UpsertExpenseAsync(RecurringExpense expense);
}
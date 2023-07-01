using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseWriter: CosmosRepositoryBase<RecurringExpense>, IRecurringExpenseWriterRepository
{
    public RecurringExpenseWriter(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<RecurringExpenseWriter> logger) : base(client, options, CosmosTypeConstants.SpbsRecurringExpenses, logger)
    { }

    public Task<RecurringExpense?> InsertExpenseAsync(RecurringExpense expense)
    {
        return Upsert(expense);
    }

    public Task UpdateExpenseAsync(RecurringExpense expense)
    {
        return Upsert(expense);
    }
}
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration;

namespace Spbs.Ui.Features.Expenses.Repositories;

public class ExpenseWriter: CosmosRepositoryBase<Expense>, IExpenseWriterRepository
{
    public ExpenseWriter(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<ExpenseWriter> logger) : base(client, options, CosmosTypeConstants.SpbsExpenses, logger)
    { }

    public Task<Expense?> InsertExpenseAsync(Expense expense)
    {
        return Upsert(expense);
    }

    // TODO: Combine Upsert and update
    public Task UpdateExpenseAsync(Expense expense)
    {
        return Upsert(expense);
    }
}
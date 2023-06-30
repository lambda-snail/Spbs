using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration;
using Spbs.Ui.Features.Expenses;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.DataAccess;

/// <summary>
/// Reads a large number of expenses for visualization. 
/// </summary>
public class ExpenseBatchReader : CosmosRepositoryBase<ExpenseVisualizationModel>, IExpenseBatchReader
{
    public ExpenseBatchReader(CosmosClient client, IOptions<DataConfigurationOptions> options,
        ILogger<ExpenseBatchReader> logger) : base(client, options, CosmosTypeConstants.SpbsExpenses, logger)
    {
    }

    public Task<List<ExpenseVisualizationModel>> GetAllExpensesByUserForMonth(Guid userId, DateOnly month)
    {
        var monthStart = new DateTime(month.Year, month.Month, 1);
        var monthEnd = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));

        return GetAllExpensesByUserBetweenDates(userId, monthStart, monthEnd);
    }

    public Task<List<ExpenseVisualizationModel>> GetAllExpensesByUserBetweenDates(Guid userId, DateTime fromDate,
        DateTime? toDate)
    {
        if (toDate is not null)
        {
            _logger.LogInformation(
                "(Expense Batch Reader) Request to get all expenses for {UserId} between {FromYear}-{FromMonth} and {ToYear}-{ToMonth}",
                userId,
                fromDate.Year.ToString(), fromDate.Month.ToString(), toDate?.Year.ToString(), toDate?.Month.ToString());
        }
        else
        {
            _logger.LogInformation(
                "(Expense Batch Reader) Request to get all expenses for {UserId} from {FromYear}-{FromMonth}", userId,
                fromDate.Year.ToString(), fromDate.Month.ToString());
        }

        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<ExpenseVisualizationModel>>()
            .Where(doc => doc.Data.UserId == userId)
            .Where(doc => doc.Type == _cosmosType)
            .Where(doc => doc.Data.Date >= fromDate);

        if (toDate is not null)
        {
            queryDefinition = queryDefinition.Where(doc => doc.Data.Date >= fromDate && doc.Data.Date <= toDate);
        }

        return GetAll(
            queryDefinition.OrderBy(doc => doc.Data.Date)
                .ToQueryDefinition()
        );
    }
}
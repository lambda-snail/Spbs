using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Utilities;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration;

namespace Spbs.Ui.Features.RecurringExpenses;

public class RecurringExpenseReader: CosmosRepositoryBase<RecurringExpense>, IRecurringExpenseReaderRepository
{
    public RecurringExpenseReader(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<RecurringExpenseReader> logger) : base(client, options, CosmosTypeConstants.SpbsRecurringExpenses, logger)
    {
    }

    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId)
    {
        _logger.LogInformation("Request to get recurring expenses for {UserId}", userId);

        List<RecurringExpense> items = new();
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<RecurringExpense>>()
            .Where(doc => doc.Data.UserId == userId)
            .Where(doc => doc.Type == _cosmosType)
            .ToQueryDefinition();

        return GetAll(queryDefinition);
    }

    public Task<List<RecurringExpense>> GetRecurringExpensesByUserId(Guid userId, RecurrenceType type)
    {
        _logger.LogInformation("Request to get recurring expenses of type {RecurrenceType} for {UserId}", type, userId);

        List<RecurringExpense> items = new();
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<RecurringExpense>>()
            .Where(doc => doc.Type == _cosmosType)
            .Where(doc => doc.Data.UserId == userId)
            .Where(doc => doc.Data.RecurrenceType == type)
            .ToQueryDefinition();

        return GetAll(queryDefinition);
    }

    public Task<List<RecurringExpense>> GetRecurringExpensesAfterDay(Guid userId, int day)
    {
        _logger.LogInformation("Request to get recurring expenses after day {Day} for {UserId}", day, userId);

        List<RecurringExpense> items = new();
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<RecurringExpense>>()
            .Where(doc => doc.Type == _cosmosType)
            .Where(doc => doc.Data.UserId == userId)
            .Where(doc => doc.Data.BillingDay > day)
            .ToQueryDefinition();

        return GetAll(queryDefinition);
    }

    public async Task<RecurringExpense?> GetByIdAsync(Guid userId, Guid expenseId)
    {
        string id = expenseId.ToString();
        _logger.LogInformation("Request to get recurring expense {ExpenseId} for {UserId}", id, userId);

        var response = await _container.ReadItemAsync<CosmosDocument<RecurringExpense>>(id, new PartitionKey(id));

        _logger.LogInformation("Response with status {StatusCode} received in {Time} ms with RU cost of {RU}", response.StatusCode, response.Diagnostics.GetClientElapsedTime(), response.RequestCharge);
        if (!response.StatusCode.IsSuccessStatusCode())
        {
            return null;
        }

        if (response.Resource.Data.UserId != userId)
        {
            _logger.LogError("Error, requesting user {RequestingUserId} does not match the owner of the document {ActualOwningUserId}", userId, response.Resource.Data.UserId);
            return null;
        }
        
        return response.Resource.Data;
    }
}

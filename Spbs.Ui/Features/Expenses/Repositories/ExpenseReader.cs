using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Ui.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration;

namespace Spbs.Ui.Features.Expenses.Repositories;

public class ExpenseReader : CosmosRepositoryBase<Expense>, IExpenseReaderRepository
{
    public ExpenseReader(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<ExpenseReader> logger) : base(client, options, CosmosTypeConstants.SpbsExpenses, logger) { }

    public Task<List<Expense>> GetSingleExpensesByUser(Guid userId, int take = 25, int skip = 0)
    {
        _logger.LogInformation("(Expense Reader) Request to get expenses for {UserId}", userId);

        List<Expense> items = new(take);
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<Expense>>()
            .Where(doc => doc.Data.UserId == userId)
            .Skip(skip)
            .Take(take)
            .ToQueryDefinition();

        return GetAll(queryDefinition);
    }

    public async Task<Expense?> GetUserExpenseById(Guid expenseId, Guid userId)
    {
        _logger.LogInformation("(Expense Reader) Request to get expense {ExpenseId} for {UserId}", expenseId, userId);

        var expense = await GetById(expenseId);
        if (expense?.UserId == userId)
        {
            _logger.LogError("(Eula Reader) Attempted to get expense {LinkId} for user {SoughtUserId}, but expense was actually for user {ActualUserId}", expenseId, userId, expense.UserId);
            return expense;
        }

        return null;
    }

    public Task<List<Expense>> GetAllExpensesByUserFromMonth(Guid userId, DateTime date)
    {
        _logger.LogInformation("(Expense Reader) Request to get all expenses for {UserId} from date {Date}", userId, date);
        
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<Expense>>()
            .Where(doc => doc.Data.UserId == userId)
            .Where(doc => doc.Type == _cosmosType)
            .Where(doc => doc.Data.Date >= date)
            .OrderBy(doc => doc.Data.Date)
            .ToQueryDefinition();

        return GetAll(queryDefinition);
    }
    
    public Task<List<Expense>> GetSingleExpensesByUserFromMonth(Guid userId, DateTime date, int take, int skip)
    {
        _logger.LogInformation("(Expense Reader) Request to get expenses for {UserId} from date {Date}", userId, date);
        
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<Expense>>()
            .Where(doc => doc.Data.UserId == userId)
            .Where(doc => doc.Type == _cosmosType)
            .Where(doc => doc.Data.Date >= date)
            .OrderBy(doc => doc.Data.Date)
            .Skip(skip)
            .Take(take)
            .ToQueryDefinition();

        return GetAll(queryDefinition);
    }
}
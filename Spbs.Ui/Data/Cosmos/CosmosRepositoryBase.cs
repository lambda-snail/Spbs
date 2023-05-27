#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Utilities;
using Spbs.Shared.Data;
using Spbs.Ui.Data.Cosmos;

namespace Spbs.Ui.Features.BankIntegration;

public interface ICosmosReader<T>
{
    Task<T?> GetById(Guid id);
    Task<ReadOnlyCollection<T>> GetAllForUser(Guid userId);
}

public class CosmosRepositoryBase<T> where T : class, ICosmosData
{
    private readonly ILogger<CosmosRepositoryBase<T>> _baseLogger;
    protected readonly Container _container;
    protected readonly Database _database;

    public CosmosRepositoryBase(CosmosClient client, IOptions<DataConfigurationOptions> options,
        ILogger<CosmosRepositoryBase<T>> baseLogger)
    {
        _baseLogger = baseLogger;
        _database = client.GetDatabase(options.Value.DatabaseName);
        _container = _database.GetContainer(options.Value.DataContainerName);
        ArgumentNullException.ThrowIfNull(_container);
    }

    /// <summary>
    /// Get an item using a point read.
    /// </summary>
    public async Task<T?> GetById(Guid id)
    {
        _baseLogger.LogInformation("(Base Repository) Request to get {T} with id {Id}", id);

        string strId = id.ToString();
        var response = await _container.ReadItemAsync<CosmosDocument<T>>(strId, new PartitionKey(strId));

        _baseLogger.LogInformation(
            "(Base Repository) Response recieved in {ResponseTime} with status code {StatusCode}",
            response.Diagnostics.GetClientElapsedTime(), response.StatusCode);
        if (response.StatusCode.IsSuccessStatusCode())
        {
            return response.Resource.Data;
        }

        return null;
    }

    /// <summary>
    /// Retrieve all items of a given type belonging to the user.
    /// </summary>
    public async Task<ReadOnlyCollection<T>> GetAllForUser(Guid userId)
    {
        _baseLogger.LogInformation("(Base Repository) Request to get links for {UserId}", userId);

        List<T> items = new();
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<T>>()
            .Where(doc => doc.Data.UserId == userId)
            .ToQueryDefinition();

        items = await GetAll(queryDefinition);

        return new ReadOnlyCollection<T>(items);
    }

    // public async Task<ulong> CountItemsForUser()
    // {
    //     
    // }

    protected async Task<List<T>> GetAll(QueryDefinition query)
    {
        List<T> items = new();
        using FeedIterator<CosmosDocument<T>>
            feedIterator = _container.GetItemQueryIterator<CosmosDocument<T>>(query);

        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync();
            _baseLogger.LogInformation(
                "(Base Repository) Response recieved in {ResponseTime} with status code {StatusCode}",
                response.Diagnostics.GetClientElapsedTime(), response.StatusCode);
            
            if (!response.StatusCode.IsSuccessStatusCode())
            {
                break;
            }
            
            foreach (var item in response)
            {
                items.Add(item.Data);
            }
        }

        return items;
    }
}
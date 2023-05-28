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
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public interface ICosmosReader<T>
{
    Task<T?> GetById(Guid id);
    Task<ReadOnlyCollection<T>> GetAllForUser(Guid userId);
}

public class CosmosRepositoryBase<T> : ICosmosReader<T> 
    where T : class, ICosmosData
{
    protected readonly ILogger<CosmosRepositoryBase<T>> _logger;
    protected readonly Container _container;
    protected readonly Database _database;

    protected readonly string _cosmosType;
    
    public CosmosRepositoryBase(
        CosmosClient client, 
        IOptions<DataConfigurationOptions> options, 
        string cosmosType,
        ILogger<CosmosRepositoryBase<T>> logger)
    {
        _cosmosType = cosmosType;
        _logger = logger;
        _database = client.GetDatabase(options.Value.DatabaseName);
        _container = _database.GetContainer(options.Value.DataContainerName);
        ArgumentNullException.ThrowIfNull(_container);
    }

    /// <summary>
    /// Get an item using a point read.
    /// </summary>
    public async Task<T?> GetById(Guid id)
    {
        _logger.LogInformation("(Base Repository) Request to get {T} with id {Id}", id);

        string strId = id.ToString();
        var response = await _container.ReadItemAsync<CosmosDocument<T>>(strId, new PartitionKey(strId));

        _logger.LogInformation(
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
        _logger.LogInformation("(Base Repository) Request to get links for {UserId}", userId);
        
        var queryDefinition = _container.GetItemLinqQueryable<CosmosDocument<T>>()
            .Where(doc => doc.Data.UserId == userId)
            .ToQueryDefinition();

        List<T> items = await GetAll(queryDefinition);
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
            _logger.LogInformation(
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

    public async Task<T?> Upsert(T item)
    {
        bool isCreate = false;
        if (item.Id == Guid.Empty)
        {
            isCreate = true;
            item.Id = Guid.NewGuid();
        }
        
        _logger.LogInformation("(Repository Base) Upsert {T} with id {ItemId}, {IsCreate}", typeof(T), item.Id, isCreate);
        var model = new CosmosDocument<T>
        {
            Id = item.Id,
            Type = _cosmosType,
            Data = item
        };
        
        var response = await _container.UpsertItemAsync(model);
        if (response.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogInformation("(Repository Base) Successfully upserted {T} with id {ItemId}", typeof(T), response.Resource.Id);
            return response.Resource.Data;            
        }

        return null;
    }
}
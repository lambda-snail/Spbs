using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;

namespace Spbs.Ui.Features.BankIntegration;

public class CosmosRepositoryBase
{
    protected readonly Container _container;
    protected readonly Database _database;

    public CosmosRepositoryBase(CosmosClient client, IOptions<DataConfigurationOptions> options)
    {
        _database = client.GetDatabase(options.Value.DatabaseName);
        _container = _database.GetContainer(options.Value.DataContainerName);
        ArgumentNullException.ThrowIfNull(_container);
    }
}
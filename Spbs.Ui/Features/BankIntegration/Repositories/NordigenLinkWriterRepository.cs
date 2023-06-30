using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public class NordigenLinkWriterRepository : CosmosRepositoryBase<NordigenLink>, INordigenLinkWriterRepository
{
    public NordigenLinkWriterRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenLinkWriterRepository> logger) : base(client, options, CosmosTypeConstants.NordigenLink, logger)
    { }
    
    /// <summary>
    /// Inserts a lnk into the database. If the link already exists then nothing happens.
    /// </summary>
    public async Task<NordigenLink?> UpsertLink(NordigenLink link)
    {
        var response = await _UpsertLinkDocument(link);

        // TODO: Handle errors
        return response.Resource.Data;
    }

    // TODO: Remove from user document
    public Task DeleteLink(NordigenLink link)
    {
        string idString = link.Id.ToString();
        return _container.DeleteItemAsync<CosmosDocument<NordigenLink>>(idString, new PartitionKey(idString));
    }

    private Task<ItemResponse<CosmosDocument<NordigenLink>>> _UpsertLinkDocument(NordigenLink link)
    {
        if (link.Id == Guid.Empty)
        {
            link.Id = Guid.NewGuid();
        }
        
        var model = new CosmosDocument<NordigenLink>
        {
            Id = link.Id,
            Type = _cosmosType,
            Data = link
        };
        
        return _container.UpsertItemAsync(model);
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;
using User = Spbs.Ui.Features.BankIntegration.Models.User;

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
        if (response is { StatusCode: HttpStatusCode.Created }) // Only link to user if we created a new link
        {
            link.Id = response.Resource.Data.Id;
            await LinkToUserDocument(link);
            return link;
        }
        
        // TODO: Handle errors
        return null;
    }

    // TODO: Remove from user document
    public Task DeleteLink(NordigenLink link)
    {
        string idString = link.Id.ToString();
        return _container.DeleteItemAsync<CosmosDocument<NordigenLink>>(idString, new PartitionKey(idString));
    }

    private async Task LinkToUserDocument(NordigenLink link)
    {
        var queryResult = _container.GetItemLinqQueryable<CosmosDocument<User>>()
            .Where(l => l.Id == link.UserId)
            .ToFeedIterator();

        var result = await queryResult.ReadNextAsync();
        if (result is { Count: > 1 })
        {
            _logger.LogError("Found more than one user documents for {LinkId} and {UserId}", link.Id, link.UserId);
            return;
        }
        
        CosmosDocument<User>? user = result.FirstOrDefault();
        if (user is null)
        {
            // TODO: Log user not found
            return;
        }

        if (user.Data.NordigenLinks.Any(l => l == link.Id))
        {
            return;
        }

        user.Data.NordigenLinks.Add(link.Id);
        await _container.UpsertItemAsync(user);
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
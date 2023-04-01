using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Ui.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public interface INordigenLinkReaderRepository
{
    Task<NordigenLink?> GetLinkById(Guid id, Guid userId);
    Task<ReadOnlyCollection<NordigenLink>> GetLinksForUser(Guid userId);
}

public class NordigenLinkReaderRepository : CosmosRepositoryBase, INordigenLinkReaderRepository
{
    public NordigenLinkReaderRepository(CosmosClient client, IOptions<DataConfigurationOptions> options)
        :base(client, options) {}

    public async Task<NordigenLink?> GetLinkById(Guid id, Guid userId)
    {
        // TODO: Add loggin and error handling
        var feedIterator = _container.GetItemLinqQueryable<CosmosDocument<NordigenLink>>()
            .Where(doc => doc.Id == id && doc.Data.UserId == userId)
            .ToFeedIterator();
        var response = await feedIterator.ReadNextAsync();
        return response.FirstOrDefault()?.Data; // Should be at most one
    }

    public async Task<ReadOnlyCollection<NordigenLink>> GetLinksForUser(Guid userId)
    {
        List<NordigenLink> userLinks = new();
        var feedIterator = _container.GetItemLinqQueryable<CosmosDocument<NordigenLink>>()
            .Where(doc => doc.Data.UserId == userId)
            .ToFeedIterator();

        var response = await feedIterator.ReadNextAsync();
        if (response is not { StatusCode: >= HttpStatusCode.OK and <= (HttpStatusCode)299 })
        {
            return new ReadOnlyCollection<NordigenLink>(userLinks);
        }
        
        foreach (var cosmosDocument in response)
        {
            if (cosmosDocument is not null)
            {
                userLinks.Add(cosmosDocument.Data);
            }
        }

        return new ReadOnlyCollection<NordigenLink>(userLinks);
    }
}

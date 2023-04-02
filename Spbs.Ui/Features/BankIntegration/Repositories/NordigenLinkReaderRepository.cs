using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<NordigenLinkReaderRepository> _logger;

    public NordigenLinkReaderRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenLinkReaderRepository> logger)
        :base(client, options)
    {
        _logger = logger;
    }

    public async Task<NordigenLink?> GetLinkById(Guid id, Guid userId)
    {
        _logger.LogInformation("(Link Reader) Request to get link {LinkId} for {UserId}", id, userId);
        
        var feedIterator = _container.GetItemLinqQueryable<CosmosDocument<NordigenLink>>()
            .Where(doc => doc.Id == id && doc.Data.UserId == userId)
            .ToFeedIterator();
        
        var response = await feedIterator.ReadNextAsync();
        _logger.LogInformation("(Link Reader) Response recieved in {ResponseTime} with status code {StatusCode}", response.Diagnostics.GetClientElapsedTime(), response.StatusCode);
        
        return response.FirstOrDefault()?.Data; // Should be at most one
    }

    public async Task<ReadOnlyCollection<NordigenLink>> GetLinksForUser(Guid userId)
    {
        _logger.LogInformation("(Link Reader) Request to get links for {UserId}", userId);
        
        List<NordigenLink> userLinks = new();
        var feedIterator = _container.GetItemLinqQueryable<CosmosDocument<NordigenLink>>()
            .Where(doc => doc.Data.UserId == userId)
            .ToFeedIterator();

        var response = await feedIterator.ReadNextAsync();
        
        _logger.LogInformation("(Link Reader) Response recieved in {ResponseTime} with status code {StatusCode}", response.Diagnostics.GetClientElapsedTime(), response.StatusCode);
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

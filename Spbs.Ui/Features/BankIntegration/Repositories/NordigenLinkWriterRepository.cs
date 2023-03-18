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
using Spbs.Ui.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;
using User = Spbs.Ui.Features.BankIntegration.Models.User;

namespace Spbs.Ui.Features.BankIntegration;

public class NordigenLinkWriterRepository : CosmosRepositoryBase, INordigenLinkWriterRepository
{
    private readonly ILogger<NordigenEulaWriterRepository> _logger;
    
    public NordigenLinkWriterRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenEulaWriterRepository> logger) : base(client, options)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Inserts a lnk into the database. If the link already exists then nothing happens.
    /// </summary>
    public async Task<NordigenLink?> Upsert(NordigenLink link)
    {
        var response = await UpsertLinkDocument(link);
        if (response is { StatusCode: >= HttpStatusCode.OK and <HttpStatusCode.MultipleChoices })
        {
            link.Id = response.Resource.Data.Id;
            await LinkToUserDocument(link);
            return link;
        }
        
        // TODO: Handle errors
        return null;
    }

    private async Task LinkToUserDocument(NordigenLink link)
    {
        var queryResult = _container.GetItemLinqQueryable<CosmosDocument<User>>()
            .Where(l => l.Data.UserId == link.UserId)
            .ToFeedIterator();

        var result = await queryResult.ReadNextAsync();
        if (result is { Count: > 0 })
        {
            _logger.LogError("Found more than one user lnk documents for {UserId}", link.UserId);
            return;
        }

        CosmosDocument<User>? links = result.FirstOrDefault();
        if (links is null)
        {
            links = new()
            {
                Id = Guid.NewGuid(),
                Type = CosmosTypeConstants.NordigenUserLinks,
                Data = new()
            };
        }

        if (links.Data.NordigenLinks.Any(l => l == link.Id))
        {
            return;
        }

        links.Data.NordigenLinks.Add(link.Id);
        await _container.UpsertItemAsync(links);
    }

    private Task<ItemResponse<CosmosDocument<NordigenLink>>> UpsertLinkDocument(NordigenLink link)
    {
        if (link.Id == Guid.Empty)
        {
            link.Id = Guid.NewGuid();
        }
        
        var model = new CosmosDocument<NordigenLink>
        {
            Id = link.Id,
            Type = CosmosTypeConstants.NordigenLink,
            Data = link
        };
        
        return _container.UpsertItemAsync(model);
    }
}
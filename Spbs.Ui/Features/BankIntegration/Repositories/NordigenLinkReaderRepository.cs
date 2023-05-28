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

public class NordigenLinkReaderRepository : CosmosRepositoryBase<NordigenLink>, INordigenLinkReaderRepository
{
    public NordigenLinkReaderRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenLinkReaderRepository> logger)
        :base(client, options, CosmosTypeConstants.NordigenLink, logger) { }

    public async Task<NordigenLink?> GetLinkById(Guid id, Guid userId)
    {
        _logger.LogInformation("(Link Reader) Request to get link {LinkId} for {UserId}", id, userId);

        var link = await GetById(id);
        if (link?.UserId == userId)
        {
            _logger.LogError("Attempted to get link {LinkId} for user {SoughtUserId}, but link was actually for user {ActualUserId}", id, userId, link.UserId);
            return link;
        }

        return null;
    }

    public Task<ReadOnlyCollection<NordigenLink>> GetLinksForUser(Guid userId)
    {
        _logger.LogInformation("(Link Reader) Request to get links for {UserId}", userId);
        return GetAllForUser(userId);
    }
}

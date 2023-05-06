using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Ui.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public class NordigenEulaReaderRepository : CosmosRepositoryBase, INordigenEulaReaderRepository
{
    public NordigenEulaReaderRepository(CosmosClient client, IOptions<DataConfigurationOptions> options)
        :base(client, options) {}

    public async Task<NordigenEula?> GetEulaById(Guid id, Guid userId)
    {
        // TODO: Add loggin and error handling
        var feedIterator = _container.GetItemLinqQueryable<CosmosDocument<NordigenEula>>()
                                     .Where(doc => doc.Id == id && doc.Data.UserId == userId)
                                     .ToFeedIterator();
        var response = await feedIterator.ReadNextAsync();
        return response.FirstOrDefault()?.Data; // Should only be one
    }
}
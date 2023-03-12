using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public class NordigenEulaReaderRepository : CosmosRepositoryBase, INordigenEulaReaderRepository
{
    public NordigenEulaReaderRepository(CosmosClient client, IOptions<DataConfigurationOptions> options)
        :base(client, options) {}

    public async Task<NordigenEula> GetEulaById(Guid id, Guid userId)
    {
        // TODO: Add loggin and error handling
        var response = _container.GetItemLinqQueryable<NordigenEula>().Where(eula => eula.Id == id && eula.UserId == userId).ToFeedIterator();
        var x = await response.ReadNextAsync();
        return x.FirstOrDefault(); // Should only be one
    }
}
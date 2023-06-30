using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public class NordigenEulaReaderRepository : CosmosRepositoryBase<NordigenEula>, INordigenEulaReaderRepository
{
    public NordigenEulaReaderRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenEulaReaderRepository> logger)
        :base(client, options, CosmosTypeConstants.NordigenEula, logger) {}

    public async Task<NordigenEula?> GetEulaById(Guid id, Guid userId)
    {
        _logger.LogInformation("(Eula Reader) Request to get eula {EulaId} for {UserId}", id, userId);

        var eula = await GetById(id);
        if (eula?.UserId == userId)
        {
            _logger.LogError("(Eula Reader) Attempted to get eula {LinkId} for user {SoughtUserId}, but eula was actually for user {ActualUserId}", id, userId, eula.UserId);
            return eula;
        }

        return null;
    }
}
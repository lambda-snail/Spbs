using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SerilogTimings.Extensions;
using Spbs.Shared.Data;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public record struct EulaWritingError(string Message);

public class NordigenEulaWriterRepository : CosmosRepositoryBase, INordigenEulaWriterRepository
{
    private readonly ILogger<NordigenEulaWriterRepository> _logger;

    public NordigenEulaWriterRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenEulaWriterRepository> logger)
        :base(client, options)
    {
        _logger = logger;
    }

    public async Task<NordigenEula> Upsert(NordigenEula eula)
    {
        var response = await _container.UpsertItemAsync(eula);
        
        _logger.LogInformation("Completed upsert of ({EulaId}) by user ({UserId}), with status {Status} and cost {RequestCharge}", eula.Id, eula.UserId, response.StatusCode, response.RequestCharge);
        return response.Resource;
    }
}
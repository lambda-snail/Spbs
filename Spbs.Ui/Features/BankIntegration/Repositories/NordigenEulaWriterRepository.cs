using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SerilogTimings.Extensions;
using Spbs.Shared.Data;
using Spbs.Ui.Data.Cosmos;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public record struct EulaWritingError(string Message);

public class NordigenEulaWriterRepository : CosmosRepositoryBase<NordigenEula>, INordigenEulaWriterRepository
{
    public NordigenEulaWriterRepository(CosmosClient client, IOptions<DataConfigurationOptions> options, ILogger<NordigenEulaWriterRepository> logger)
        :base(client, options, CosmosTypeConstants.NordigenEula, logger) { }

    public async Task<NordigenEula> Upsert(NordigenEula eula)
    {
        var model = new CosmosDocument<NordigenEula>
        {
            Id = (eula.Id == default? Guid.NewGuid() : eula.Id),
            Type = CosmosTypeConstants.NordigenEula,
            Data = eula
        };
        
        var response = await _container.UpsertItemAsync(model);
        
        _logger.LogInformation("Completed upsert of ({EulaId}) by user ({UserId}) in {ElapsedTime}, with status {Status} and cost {RequestCharge}", eula.Id, eula.UserId, response.Diagnostics.GetClientElapsedTime(), response.StatusCode, response.RequestCharge);
        return response.Resource.Data;
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Spbs.Shared.Data;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public record struct EulaWritingError(string Message);

public class NordigenEulaWriterRepository : INordigenEulaWriterRepository
{
    private readonly Container _container;

    public NordigenEulaWriterRepository(CosmosClient client, IOptions<DataConfigurationOptions> options)
    {
        Database database = client.GetDatabase(options.Value.DatabaseName);
        _container = database.GetContainer(options.Value.DataContainerName);
        ArgumentNullException.ThrowIfNull(_container);
    }

    public async Task<NordigenEula> Upsert(NordigenEula eula)
    {
        var response = await _container.UpsertItemAsync(eula);
        return response.Resource;
    }
}
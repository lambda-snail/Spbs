using System;
using System.Threading.Tasks;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Services;

namespace Spbs.Ui.Features.BankIntegration;

/// <summary>
/// Gathers dependencies and groups common operations pertaining to agreements needed to connect to Nordigen.
/// </summary>
public class EulaService : IEulaService
{
    private readonly INordigenEulaReaderRepository _eulaReader;
    private readonly INordigenEulaWriterRepository _eulaWriter;
    private readonly ILogger<EulaService> _logger;

    public EulaService(
        INordigenApiClient nordigenClient, 
        INordigenEulaReaderRepository eulaReader, 
        INordigenEulaWriterRepository eulaWriter, 
        ILogger<EulaService> logger)
    {
        _eulaReader = eulaReader;
        _eulaWriter = eulaWriter;
        _logger = logger;
    }

    public Task<NordigenEula> UpsertEula(NordigenEula eula)
    {
        return _eulaWriter.Upsert(eula);
    }

    public Task<NordigenEula?> GetEula(Guid eulaId, Guid userId)
    {
        return _eulaReader.GetEulaById(eulaId, userId);
    }

    // public Task SendEulaToNordigen(NordigenEula eula)
    // {
    //     
    // }
}
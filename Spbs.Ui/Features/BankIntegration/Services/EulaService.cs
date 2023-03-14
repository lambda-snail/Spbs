using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    private readonly INordigenApiClient _nordigenClient;
    private readonly INordigenEulaReaderRepository _eulaReader;
    private readonly INordigenEulaWriterRepository _eulaWriter;
    private readonly IMapper _mapper;
    private readonly ILogger<EulaService> _logger;

    public EulaService(
        INordigenApiClient nordigenClient, 
        INordigenEulaReaderRepository eulaReader, 
        INordigenEulaWriterRepository eulaWriter, 
        IMapper mapper,
        ILogger<EulaService> logger)
    {
        _nordigenClient = nordigenClient;
        _eulaReader = eulaReader;
        _eulaWriter = eulaWriter;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Save the agreement to our internal database. Not to be confused with updating the agreement in Nordigen. 
    /// </summary>
    /// <param name="eula"></param>
    /// <returns></returns>
    public Task<NordigenEula> UpsertEulaToDatabase(NordigenEula eula)
    {
        return _eulaWriter.Upsert(eula);
    }

    public Task<NordigenEula?> GetEulaFromDatabase(Guid eulaId, Guid userId)
    {
        return _eulaReader.GetEulaById(eulaId, userId);
    }

    /// <summary>
    /// Send a newly created eula to Nordigen, assuming that it has been accepted.
    /// </summary>
    /// <param name="eula"></param>
    public async Task<NordigenEula> CreateEulaWithNordigen(NordigenEula eula)
    {
        // TODO Add logic here and in the api service
        var createdEula = await _nordigenClient.CreateEndUserAgreement(
            eula.InstitutionId,
            eula.MaxHistoricalDays,
            eula.AccessValidForDays,
            eula.AccessScope.ToList());

        // This will need to be fetched again later to determine if the agreement has been agreed upon, so we don't
        // save it to the database here.
        return  _mapper.Map<NordigenEula>(createdEula);
    }
}
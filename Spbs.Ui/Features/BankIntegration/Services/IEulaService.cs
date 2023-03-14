using System;
using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface IEulaService
{
    Task<NordigenEula> UpsertEulaToDatabase(NordigenEula eula);
    Task<NordigenEula?> GetEulaFromDatabase(Guid eulaId, Guid userId);
    Task<NordigenEula> CreateEulaWithNordigen(NordigenEula eula);
}
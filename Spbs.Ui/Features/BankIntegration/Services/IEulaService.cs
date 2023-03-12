using System;
using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface IEulaService
{
    Task<NordigenEula> UpsertEula(NordigenEula eula);
    Task<NordigenEula?> GetEula(Guid eulaId, Guid userId);
}
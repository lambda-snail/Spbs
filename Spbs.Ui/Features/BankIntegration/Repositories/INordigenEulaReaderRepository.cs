using System;
using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public interface INordigenEulaReaderRepository
{
    Task<NordigenEula?> GetEulaById(Guid id, Guid userId);
}
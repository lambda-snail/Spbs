using System.Threading.Tasks;
using Spbs.Shared.Data;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public interface INordigenEulaWriterRepository
{
    Task<NordigenEula> Upsert(NordigenEula eula);
}
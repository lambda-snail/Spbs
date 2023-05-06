using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public interface INordigenLinkWriterRepository
{
    Task<NordigenLink?> Upsert(NordigenLink link);
    Task Delete(NordigenLink link);
}
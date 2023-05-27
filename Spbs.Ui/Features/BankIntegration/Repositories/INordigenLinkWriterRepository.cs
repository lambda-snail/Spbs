using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration;

public interface INordigenLinkWriterRepository
{
    Task<NordigenLink?> UpsertLink(NordigenLink link);
    Task DeleteLink(NordigenLink link);
}
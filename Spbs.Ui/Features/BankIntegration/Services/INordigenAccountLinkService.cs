using System;
using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface INordigenAccountLinkService
{
    Task<NordigenAccountLinkService.RedirectUrl?> CreateLink(Institution institution, NordigenEula eula, Guid userId, bool accountSelection);
    Task DeleteLink(NordigenLink link);
}
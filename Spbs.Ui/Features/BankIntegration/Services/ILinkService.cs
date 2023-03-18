using System;
using System.Threading.Tasks;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface ILinkService
{
    Task<LinkService.RedirectUrl?> CreateLink(Institution institution, NordigenEula eula, Guid userId);
}
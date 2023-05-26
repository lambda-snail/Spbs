using System;
using System.Threading.Tasks;
using Integrations.Nordigen.Models;
using Spbs.Ui.Features.BankIntegration.Models;

namespace Spbs.Ui.Features.BankIntegration.Services;

public interface INordigenAccountLinkService
{
    Task<NordigenAccountLinkService.RedirectUrl?> CreateLink(Institution institution, NordigenEula eula, Guid userId, bool accountSelection);
    Task DeleteLink(NordigenLink link);
    Task<NordigenLink?> GetLink(Guid linkId);
    Task<NordigenLink?> SaveLinkToDatabase(NordigenLink link);

    public Task<ListTransactionsResponse?> GetAccountTransactions(Guid accountId, TransactionsRequestParameters requestParameters);
}
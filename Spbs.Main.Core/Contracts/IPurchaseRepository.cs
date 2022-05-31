using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Contracts;

public interface IPurchaseRepository
{
    Task UpsertPurchase(Purchase purchase);
    Task<List<Purchase>> GetPurchasesOfUser(string userId, DateTime? since = null);
    Task DeletePurchase(int purchaseId);
    Task DeleteAllPurchasesOfUser(string userId);
}



using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Contracts;

public interface IPurchaseRepository
{
    Task InsertPurchase(Purchase purchase);
    Task UpdatePurchase(Purchase purchase);
    Task<Purchase> GetPurchaseById(Guid purchaseId);
    Task<List<Purchase>> GetPurchasesOfUser(string userId, DateTime? since = null);
    Task DeletePurchase(Guid purchaseId);
    Task DeleteAllPurchasesOfUser(string userId);
}



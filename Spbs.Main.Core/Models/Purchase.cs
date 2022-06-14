#nullable disable

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Spbs.Main.Core.Models;

public class Purchase
{
    public Guid Id { get; set; }
    public string OwnerId { get; set; }
    public DateTime PurchaseDateTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<PurchaseItem> Items { get; set; }

    public decimal Total => Items.Aggregate(0m, (sum, next) => sum + next.Price);
}
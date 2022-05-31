#nullable disable

namespace Spbs.Main.Core.Models;

public class Purchase
{
    public int Id { get; set; }
    public string OwnerId { get; set; }
    public DateTime PurchaseDateTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<PurchaseItem> Items { get; set; } 
}
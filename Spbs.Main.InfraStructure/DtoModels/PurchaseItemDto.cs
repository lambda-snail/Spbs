#nullable disable
namespace Spbs.Main.InfraStructure.DtoModels;

public class PurchaseItemDto
{
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ProductName { get; set; }
    public List<string> Categories { get; set; }
}
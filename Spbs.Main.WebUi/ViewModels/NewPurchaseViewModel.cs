using System.ComponentModel.DataAnnotations;

namespace Spbs.Main.WebUi.ViewModels;

public class NewPurchaseViewModel
{
    [Required(ErrorMessage = "Please provide a date for the purhcase")]
    public DateTime PurchaseDateTime { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Please provide a location for the purchase")]
    public string Location { get; set; }
    public string Description { get; set; }

    //public List<PurchaseItemDto> Items;
}
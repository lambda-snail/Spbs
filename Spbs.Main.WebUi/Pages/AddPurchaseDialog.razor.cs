using Spbs.Main.WebUi.ViewModels;

namespace Spbs.Main.WebUi.Pages;

public partial class AddPurchaseDialog
{
    public bool IsVisible { get; set; }
    public NewPurchaseViewModel NewPurchase { get; set; } = new NewPurchaseViewModel();
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        IsVisible = false;
    }

    public void HandleValidSubmit()
    {
        
    }

    public void HandleInvalidSubmit()
    {
        
    }

    public void ShowModal()
    {
        IsVisible = true;
        StateHasChanged();
    }

    public void CloseDialog()
    {
        IsVisible = false;
        // Callback here 
        StateHasChanged();
    }
}
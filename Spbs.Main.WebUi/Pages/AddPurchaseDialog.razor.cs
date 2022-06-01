namespace Spbs.Main.WebUi.Pages;

public partial class AddPurchaseDialog
{
    public bool IsVisible { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        IsVisible = false;
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
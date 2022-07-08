#nullable disable

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;

namespace Spbs.Main.WebUi.Pages;

[Authorize]
public partial class PurchasesOverview
{
    [Inject] public ILoggedInUserService UserService { get; set; }
    [Inject] public IMediator Mediator { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private AddPurchaseDialog _addPurchaseDialog;
    private DateTime StartDate { get; set; }
    private DateTime EndDate { get; set; }
    
    private List<Purchase> _purchases = new();

    private IList<Purchase> _selectedPurchase; // One item

    private RadzenDataGrid<Purchase> _purchasesGrid;

    private User userDetails;
    
    private bool _emptySelection => _selectedPurchase is null || _selectedPurchase.Count == 0;

        protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        StartDate = GetStartOfMonth();
        EndDate = GetEndOfMonth();
        await GetUserDetails();
        await InitListOfPurchases();
    }

        private DateTime ToUserTimeZone(DateTime dateTime)
        {
            if (userDetails is not null)
            {
                return userDetails.Settings.ToUserTimeZone(dateTime);
            }

            return dateTime;
        }

    private async Task InitListOfPurchases()
    {
        await LoadPurchases();
    }

    private async Task GetUserDetails()
    {
        Guid userId = await UserService.GetLoggedInUserId();
        var response = await Mediator.Send(new GetUserDetails.Request(UserId: userId));
        if (response.Success)
        {
            userDetails = response.User;
        }
    }

    private async Task LoadPurchases()
    {
        Guid userId = await UserService.GetLoggedInUserId();
        GetPurchaseByUserCommand.Response response = await Mediator.Send(new GetPurchaseByUserCommand.Request(UserId: userId.ToString()));
        if (response.Success)
        {
            _purchases = response.Purchases;
        }
    }

    private async Task AddNewPurchase()
    {
        _addPurchaseDialog.CloseDialog();
        await LoadPurchases();
        StateHasChanged();
    }
    
    // private void ValueChangeHandler(RangePickerEventArgs<DateTime?> args)
    // {
    //     if (args.StartDate.HasValue && args.EndDate.HasValue)
    //     {
    //         StartDate = args.StartDate.Value;
    //         EndDate = args.EndDate.Value;
    //     }
    //
    // }

    private void EditPurchase()
    {
        if (!_emptySelection)
        {
            NavigationManager.NavigateTo($"/purchases/{_selectedPurchase[0].Id}");            
        }
    }

    private void DeleteSelectedPurchase()
    {
        if (!_emptySelection)
        {
            Purchase purchase = _selectedPurchase[0];
            _purchases.Remove(purchase);
            Mediator.Send(new DeletePurchase.Request(purchase));
            _purchasesGrid.Reload();
        }
    }
    
    private DateTime GetStartOfMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    }

    private DateTime GetEndOfMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).Subtract(TimeSpan.FromDays(1));
    }

    public void ToggleAddPurchaseComponent()
    {
        _addPurchaseDialog.ShowDialog();
    }
}
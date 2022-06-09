#nullable disable

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;
using Spbs.Main.WebUi.Contracts;

namespace Spbs.Main.WebUi.Pages;

[Authorize]
public partial class PurchasesOverview
{
    [Inject]
    public ILoggedInUserService UserService { get; set; }
    [Inject]
    public IMediator Mediator { get; set; }

    private AddPurchaseDialog _addPurchaseDialog;
    private DateTime StartDate { get; set; }
    private DateTime EndDate { get; set; }
    
    private List<Purchase> _purchases = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        StartDate = GetStartOfMonth();
        EndDate = GetEndOfMonth();
        await InitListOfPurchases();
    }

    private async Task InitListOfPurchases()
    {
        await LoadPurchases();
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
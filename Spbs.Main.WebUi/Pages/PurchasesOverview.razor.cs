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

    private AddPurchaseDialog _addPurchaseComponent;
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
        Guid userId = await UserService.GetLoggedInUserId();
        GetPurchaseByUserService.Response response = await Mediator.Send(new GetPurchaseByUserService.Request(UserId: userId.ToString()));
        if (response.Success)
        {
            _purchases = response.Purchases;
        }
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
        _addPurchaseComponent.ShowModal();
    }
}
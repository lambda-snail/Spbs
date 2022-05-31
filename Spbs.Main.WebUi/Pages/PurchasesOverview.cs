#nullable disable

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;
using Spbs.Main.WebUi.Contracts;

namespace Spbs.Main.WebUi.Pages;

[Authorize]
public partial class PurchasesOverview
{
    [Inject]
    public ILoggedInUserService _userService { get; set; }
    [Inject]
    public IMediator Mediator { get; set; }

    private List<Purchase>? _purchases;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await InitListOfPurchases();
    }

    private async Task InitListOfPurchases()
    {
        Guid userId = await _userService.GetLoggedInUserId();
        GetPurchaseByUserService.Response response = await Mediator.Send(new GetPurchaseByUserService.Request(UserId: userId.ToString()));
        if (response.Success)
        {
            _purchases = response.Purchases;
        }
    }
}
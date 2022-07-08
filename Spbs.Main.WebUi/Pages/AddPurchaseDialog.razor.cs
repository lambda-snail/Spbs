using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;
using Spbs.Main.WebUi.ViewModels;

namespace Spbs.Main.WebUi.Pages;

public partial class AddPurchaseDialog
{
    [Inject] public IMapper _mapper { get; set; }
    [Inject] public IMediator _mediator { get; set; }
    [Inject] public ILoggedInUserService _userService { get; set; } 
    
    [Parameter] public Func<Task> NewPurchaseCallback { get; set; }
    
    public bool IsVisible { get; set; }
    public NewPurchaseViewModel NewPurchase { get; set; } = new();
    
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        IsVisible = false;
    }

    public async void HandleValidSubmit()
    {
        Purchase purchase = _mapper.Map<Purchase>(NewPurchase);
        Guid userId = await _userService.GetLoggedInUserId();
        purchase.OwnerId = userId.ToString();
        
        var response = await _mediator.Send(new UpsertPurchase.Request(purchase));
        if (response.Success)
        {
            await NewPurchaseCallback.Invoke();
        }
    }

    public void HandleInvalidSubmit()
    {
        
    }

    public void ShowDialog()
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
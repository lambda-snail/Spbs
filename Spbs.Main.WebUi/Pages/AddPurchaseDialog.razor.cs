using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;
using Spbs.Main.WebUi.Contracts;
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
    public List<NewPurchaseItemViewModel> NewPurchaseItems { get; set; } = new();
    
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
        
        var response = await _mediator.Send(new InsertPurchase.Request(purchase));
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


    private NewPurchaseItemViewModel? _purchaseItemToAdd { get; set; }
    private RadzenDataGrid<NewPurchaseItemViewModel> _purchaseItemGrid { get; set; }
    
    async Task SaveRow(NewPurchaseItemViewModel purchaseItem)
    {
        if (purchaseItem == _purchaseItemToAdd)
        {
            _purchaseItemToAdd = null;
        }

        await _purchaseItemGrid.UpdateRow(purchaseItem);
    }

    void CancelEdit(NewPurchaseItemViewModel purchaseItem)
    {
        if (purchaseItem == _purchaseItemToAdd)
        {
            _purchaseItemToAdd = null;
        }

        _purchaseItemGrid.CancelEditRow(purchaseItem);
    }

    async Task DeleteRow(NewPurchaseItemViewModel purchaseItem)
    {
        if (purchaseItem == _purchaseItemToAdd)
        {
            _purchaseItemToAdd = null;
        }

        if (NewPurchaseItems.Contains(purchaseItem))
        {
            NewPurchaseItems.Remove(purchaseItem);
            await _purchaseItemGrid.Reload();
        }
        else
        {
            _purchaseItemGrid.CancelEditRow(purchaseItem);
        }
    }

    async Task InsertRow()
    {
        _purchaseItemToAdd = new NewPurchaseItemViewModel();
        await _purchaseItemGrid.InsertRow(_purchaseItemToAdd);
    }

    void OnCreateRow(NewPurchaseItemViewModel newPurchaseItem)
    {
        Console.WriteLine("Inserted a new purchase item!");
    }
    
    void OnUpdateRow(NewPurchaseItemViewModel newPurchaseItem)
    {
        Console.WriteLine("Updated a purchase item!");
    }
}
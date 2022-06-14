using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;
using Spbs.Main.WebUi.ViewModels;

namespace Spbs.Main.WebUi.Pages;

public partial class PurchaseDetails
{
    [Inject] public IMediator _mediator { get; set; }
    [Parameter] public string PurchaseId { get; set; }

    private Purchase? _purchase;
    private RadzenDataGrid<PurchaseItem> _purchaseItemGrid { get; set; }
    //private List<NewPurchaseItemViewModel> NewPurchaseItems { get; set; } = new();
    private PurchaseItem? _purchaseItemToAdd { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        bool couldConvert = Guid.TryParse(PurchaseId, out Guid id);
        if (couldConvert)
        {
            var response = await _mediator.Send(new GetPurchaseById.Request(id));
            _purchase = response.Purchase;            
        }
    }
    
    async Task SaveRow(PurchaseItem purchaseItem)
    {
        if (purchaseItem == _purchaseItemToAdd)
        {
            _purchaseItemToAdd = null;
        }

        _purchase.Items.Add(purchaseItem);
        await _mediator.Send(new UpsertPurchase.Request(_purchase));
        await _purchaseItemGrid.UpdateRow(purchaseItem);
    }

    void CancelEdit(PurchaseItem purchaseItem)
    {
        if (purchaseItem == _purchaseItemToAdd)
        {
            _purchaseItemToAdd = null;
        }

        _purchaseItemGrid.CancelEditRow(purchaseItem);
    }

    async Task DeleteRow(PurchaseItem purchaseItem)
    {
        if (purchaseItem == _purchaseItemToAdd)
        {
            _purchaseItemToAdd = null;
        }

        if (_purchase.Items.Contains(purchaseItem))
        {
            _purchase.Items.Remove(purchaseItem);
            await _mediator.Send(new UpsertPurchase.Request(_purchase));
            await _purchaseItemGrid.Reload();
        }
        else
        {
            _purchaseItemGrid.CancelEditRow(purchaseItem);
        }
    }

    async Task InsertRow()
    {
        _purchaseItemToAdd = new PurchaseItem();
        await _purchaseItemGrid.InsertRow(_purchaseItemToAdd);
    }
}
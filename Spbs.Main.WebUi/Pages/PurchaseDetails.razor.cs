using MediatR;
using Microsoft.AspNetCore.Components;
using Spbs.Main.Core.Models;
using Spbs.Main.Core.Services;

namespace Spbs.Main.WebUi.Pages;

public partial class PurchaseDetails
{
    [Inject] public IMediator _mediator { get; set; }
    [Parameter] public string PurchaseId { get; set; }

    private Purchase? _purchase;

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
}
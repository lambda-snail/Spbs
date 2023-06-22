using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.Expenses.Graphs;

[AuthenticationTaskExtension]
public partial class ExpensesGraph : ComponentBase
{
    private static readonly string _canvasId = "_canvas";
    
#pragma warning disable CS8618
    [Inject] private IExpenseReaderRepository _expenseReader { get; set; }
    [Inject] IJSRuntime _jSRuntime { get; set; }
#pragma warning restore CS8618

    private List<Expense> _expenses = new();

    private IJSObjectReference _chart;
    private bool _isChartCreated = false;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isChartCreated) return;
        
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserFromMonth(userId!.Value, new DateTime(2023, 01, 01));

        var costData = _expenses.Select(e => e.Total).ToArray();
        var dates = _expenses.Select(e => e.Date.ToShortDateString()).ToArray();
        
        var config = new
        {
            Type = "bar",
            Options = new
            {
                Responsive = true,
                Scales = new
                {
                    Y = new
                    {
                        BeginAtZero = true
                    }
                }
            },
            Data = new
            {
                Labels = dates,
                Datasets = new[]
                {
                    new
                    {
                        Label = "Expenses per month",
                        Data = costData,
                    }
                }
            }
        };
        
        //await _jSRuntime!.InvokeVoidAsync("createChart", _canvasId, config);
        _chart = await _jSRuntime!.InvokeAsync<IJSObjectReference>("createChart", _canvasId, config);
        _isChartCreated = true;
    }
}
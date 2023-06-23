using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Visualization.DataAccess;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.Graphs;

[AuthenticationTaskExtension]
public partial class ExpensesGraph : ComponentBase
{
    private static readonly string _canvasId = "_canvas";
    
#pragma warning disable CS8618
    [Inject] private IExpenseBatchReader _expenseReader { get; set; }
    [Inject] IJSRuntime _jSRuntime { get; set; }
#pragma warning restore CS8618

    private List<ExpenseVisualizationModel> _expenses = new();
    private List<string> _filteredLabels = new();
    private List<double> _filteredValues = new();
    private static readonly string _noCategoryLabel = "Unassigned";
    
    private IJSObjectReference _chart;
    private bool _isChartCreated = false;
    
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isChartCreated) return;
        
        await LoadDataForMonth(2023, 6);

        FilterByCategory();
        
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
                Labels = _filteredLabels,
                Datasets = new[]
                {
                    new
                    {
                        Label = "Expenses per month",
                        Data = _filteredValues,
                    }
                }
            }
        };
        
        _chart = await _jSRuntime!.InvokeAsync<IJSObjectReference>("createChart", _canvasId, config);
        _isChartCreated = true;
    }

    private async Task LoadDataForMonth(int year, int month)
    {
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserForMonth(userId!.Value, new DateOnly(year, month, 01));

        foreach (var expense in _expenses)
        {
            if (string.IsNullOrWhiteSpace(expense.Category))
            {
                expense.Category = _noCategoryLabel;
            }
        }
    }

    private void FilterByCategory()
    {
        Dictionary<string, double> graphData = new(_expenses.Count);

        foreach (var expense in _expenses)
        {
            if (expense.Category is null)
            {
                continue;
            }
                
            if(graphData.ContainsKey(expense.Category))
            {
                graphData[expense.Category] += expense.Total;
            }
            else
            {
                graphData[expense.Category] = expense.Total;
            }
        }

        _filteredLabels = new(graphData.Keys.Count);
        _filteredValues = new(graphData.Keys.Count);
        foreach (var data in graphData)
        {
            _filteredLabels.Add(data.Key);
            _filteredValues.Add(data.Value);
        }
    }
}
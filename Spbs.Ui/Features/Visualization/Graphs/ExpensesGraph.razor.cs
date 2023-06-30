using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApexCharts;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Visualization.DataAccess;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.Graphs;

[AuthenticationTaskExtension]
public partial class ExpensesGraph : ComponentBase
{
#pragma warning disable CS8618
    [Inject] private IExpenseBatchReader _expenseReader { get; set; }
    private ApexChart<ExpenseVisualizationModel> _chart;
    private ApexChartOptions<ExpenseVisualizationModel> _chartOptions;
#pragma warning restore CS8618

    private List<ExpenseVisualizationModel> _expenses = new();
    private const string _noCategoryLabel = "Unassigned";

    protected override async Task OnInitializedAsync()
    {
        await LoadDataForMonth(2023, 6);

        _chartOptions = new()
        {
            DataLabels = new()
            {
                Style = new()
                {
                    Colors = new() { "#FFFFFF" }
                }
            },
            Legend = new()
            {
                Show = true
            },
            Title = new()
            {
                Style = new()
                {
                    Color = "#FFFFFF"
                }
            }
        };
        
        StateHasChanged();
        await _chart.RenderAsync();
    }

    private async Task LoadDataForMonth(int year, int month)
    {
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserForMonth(userId!.Value, new DateOnly(year, month, 01));
        EnsureNoEmptyCategory();
    }

    private void EnsureNoEmptyCategory()
    {
        foreach (var expense in _expenses)
        {
            if (string.IsNullOrWhiteSpace(expense.Category))
            {
                expense.Category = _noCategoryLabel;
            }
        }
    }

    // private void SumByCategory()
    // {
    //     Dictionary<string, double> graphData = new(_expenses.Count);
    //
    //     foreach (var item in _expenses)
    //     {
    //         double value = item.Total;
    //         string label = string.IsNullOrWhiteSpace(item.Category) ? _noCategoryLabel : item.Category;
    //
    //         if(graphData.ContainsKey(label))
    //         {
    //             graphData[label] += value;
    //         }
    //         else
    //         {
    //             graphData[label] = value;
    //         }
    //     }
    //
    //     _filteredLabels = new(graphData.Keys.Count);
    //     _filteredValues = new(graphData.Keys.Count);
    //     foreach (var data in graphData)
    //     {
    //         _filteredLabels.Add(data.Key);
    //         _filteredValues.Add(data.Value);
    //     }
    // }
}
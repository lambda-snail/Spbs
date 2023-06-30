using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Visualization.DataAccess;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.Graphs;

[AuthenticationTaskExtension]
public partial class ExpensesGraph : ComponentBase
{
#pragma warning disable CS8618
    [Inject] private IExpenseBatchReader _expenseReader { get; set; }

    //private MudChart _chart1;
    //private BarChart _chart1;
    
#pragma warning restore CS8618

    private List<ExpenseVisualizationModel> _expenses = new();

    private readonly GraphDataFilter _filter = new() { FromDate = DateTime.Now };
    private List<string> _filteredLabels = new();
    private List<double> _filteredValues = new();
    private List<ChartSeries> _series = new();
    private const string _noCategoryLabel = "Unassigned";

    protected override async Task OnInitializedAsync()
    {
        await LoadDataForMonth(2023, 6);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        return RenderExpenseChart();
    }

    private async Task LoadDataForMonth(int year, int month)
    {
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserForMonth(userId!.Value, new DateOnly(year, month, 01));
    }

    private async Task LoadDataForDates(DateTime fromDate, DateTime? toDate)
    {
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserBetweenDates(userId!.Value, fromDate, toDate);
    }

    private async Task RefreshData()
    {
        await LoadDataForDates(_filter.FromDate, _filter.ToDate);
        await RenderExpenseChart();
    }

    private void SumByCategory()
    {
        Dictionary<string, double> graphData = new(_expenses.Count);

        foreach (var item in _expenses)
        {
            double value = item.Total;
            string label = string.IsNullOrWhiteSpace(item.Category) ? _noCategoryLabel : item.Category;

            if(graphData.ContainsKey(label))
            {
                graphData[label] += value;
            }
            else
            {
                graphData[label] = value;
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
    
    private async Task RenderExpenseChart()
    {
        SumByCategory();
        
        // _series = new List<ChartSeries>()
        // {
        //     new ChartSeries() { Name = "Expenses", Data = _filteredValues.ToArray() },
        //     //new ChartSeries() { Name = "Recurring Expenses", Data =  }
        // };
        
        // var data = new ChartData
        // {
        //     Labels = _filteredLabels,
        //     Datasets = new List<IChartDataset>()
        //     {
        //         new BarChartDataset()
        //         {
        //             Label = "Expenses",
        //             Data = _filteredValues,
        //             BackgroundColor = new List<string>{ "rgb(88, 80, 141)" },
        //             CategoryPercentage = 0.8,
        //             BarPercentage = 1,
        //         }
        //         // , new BarChartDataset()
        //         // {
        //         //     Label = "Recurring Expenses",
        //         //     Data = new List<double>{ 1, 0, 7, 11, 5, 2, 13, 8, 9, 10, 7, 13, 7, 5, 9, 5, 10, 5, 11, 2 },
        //         //     BackgroundColor = new List<string> { "rgb(255, 166, 0)" },
        //         //     CategoryPercentage = 0.8,
        //         //     BarPercentage = 1,
        //         // }
        //     }
        // };
        //
        // var options = new BarChartOptions();
        //
        // options.Interaction.Mode = InteractionMode.Index;
        //
        // options.Plugins.Title.Text = _filter.ToDate is null ? 
        //     "Expenses from " + _filter.FromDate.ToShortDateString() : 
        //     "Expenses Between " + _filter.FromDate.ToShortDateString() + " and " + _filter.ToDate.Value.ToShortDateString();
        // options.Plugins.Title.Display = true;
        // options.Plugins.Title.Color = "White";
        // options.Plugins.Title.Font.Size = 16;
        //
        // options.Responsive = true;
        //
        // options.Scales.X.Title.Text = "Category";
        // options.Scales.X.Title.Color = "White";
        // options.Scales.X.Title.Display = false;
        //
        // options.Scales.Y.Title.Text = "Total Cost";
        // options.Scales.Y.Title.Color = "White";
        // options.Scales.Y.Title.Display = true;
        //
        // await _chart1.UpdateAsync(data, options);
    }
}
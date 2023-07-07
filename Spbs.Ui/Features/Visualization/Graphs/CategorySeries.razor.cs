using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApexCharts;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Visualization.DataAccess;
using Spbs.Ui.Features.Visualization.Models;

namespace Spbs.Ui.Features.Visualization.Graphs;

[AuthenticationTaskExtension]
public partial class CategorySeries : ComponentBase
{
    private List<ExpenseSeriesVisualizationModel> _expenses = new();
    
    private ApexChartOptions<ExpenseSeriesVisualizationModel> _chartOptions;
    private bool _isDataLoaded = false;
    
    private List<ExpenseSeriesVisualizationModel> _filteredExpenses = new();
    private List<string> _availableCategories = new();
    private string? _filterCategory = null;
    private DateRange? _dateFilter = new(DateTime.Now.AddMonths(-1), DateTime.Now);

    private static readonly string _noCategoryPlaceholder = "Uncategorized";
    
#pragma warning disable CS8618
    private ApexChart<ExpenseSeriesVisualizationModel> _chart;
    
    [Inject] private IExpenseBatchReader<ExpenseSeriesVisualizationModel> _expenseReader { get; set; }
#pragma warning restore CS8618
    
    protected override async Task OnInitializedAsync()
    {
        _chartOptions = CommonChartOptionsFactory.CreateOptions<ExpenseSeriesVisualizationModel>();
        _chartOptions.DataLabels = new ApexCharts.DataLabels
        {
            OffsetY = -6d
        };
        
        await LoadData();
        
        StateHasChanged();
        await _chart.RenderAsync();
    }

    private async Task LoadData()
    {
        if (_dateFilter is null or { Start: null })
        {
            return;
        }
        
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserBetweenDates(userId!.Value, _dateFilter.Start.Value, _dateFilter.End);

        _availableCategories = _expenses.GroupBy(e => e.Category).Select(x => x.Key ?? _noCategoryPlaceholder).ToList();
        _filterCategory = null;
        _filteredExpenses = _expenses;
    }

    private string GetChartTitle()
    {
        return _filterCategory is null ? "All expenses over time" : _filterCategory + "expenses over time for";
    }

    private async Task OnRefreshGraphClicked()
    {
        await LoadData();
        StateHasChanged();
        await _chart.RenderAsync();
    }

    private Task OnSelectedCategoryValueChanged(string category)
    {
        _filterCategory = category;
        _filteredExpenses = string.IsNullOrWhiteSpace(category) ? _expenses : _expenses.Where(e => e.Category == _filterCategory).ToList();
        StateHasChanged();
        return _chart.RenderAsync();
    }
}
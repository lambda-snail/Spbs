using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApexCharts;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Visualization.DataAccess;
using Spbs.Ui.Features.Visualization.Models;
using Color = ApexCharts.Color;

namespace Spbs.Ui.Features.Visualization.Graphs;

[AuthenticationTaskExtension]
public partial class ExpensesGraph : ComponentBase
{
#pragma warning disable CS8618
    [Inject] private ISnackbar _snackbar { get; set; }
    [Inject] private IExpenseBatchReader _expenseReader { get; set; }
    private ApexChart<ExpenseVisualizationModel> _chart;
    private ApexChartOptions<ExpenseVisualizationModel> _chartOptions;
#pragma warning restore CS8618

    private List<ExpenseVisualizationModel> _expenses = new();
    private const string _noCategoryLabel = "Unassigned";
    
    private DateTime? _expensesMonth = DateTime.Now;
    private SeriesType _chartType = SeriesType.Donut;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataForMonth(2023, 6);

        _chartOptions = new()
        {
            DataLabels = new()
            {
                Enabled = true,
                Style = new()
                {
                    Colors = new() { "#FFFFFF" }
                }
            },
            PlotOptions = new()
            {
                Bar = new()
                {
                    BorderRadius = 4,
                    Horizontal = true,
                    DataLabels = new()
                    {
                        Position = "top",
                        Total = new()
                        {
                            Enabled = true,
                            Style = new()
                            {
                                Color = "#FFFFFF"
                            }
                        }
                    }
                },
                Pie = new()
                {
                    Donut = new()
                    {
                        Labels = new()
                        {
                            Name = new()
                            {
                                Color = "#FFFFFF"
                            }
                        }
                    }
                }
            },
            Legend = new()
            {
                Show = true,
                Labels = new()
                {
                    Colors = new Color("#FFFFFF")
                }
            },
            Title = new()
            {
                Style = new()
                {
                    Color = "#FFFFFF"
                }
            },
            Xaxis = new()
            {
                Labels = new()
                {
                    Style = new()
                    {
                        Colors = new Color("#FFFFFF")
                    }
                }
            },
            Yaxis = new()
            {
                new()
                {
                    Labels = new()
                    {
                        Style = new()
                        {
                            Colors = new Color("#FFFFFF")
                        }
                    }
                }
            },
            Chart = new()
            {
                Toolbar = new()
                {
                    Show = false
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

    private async Task OnRefreshGraphClicked()
    {
        if (_expensesMonth is null)
        {
            _snackbar.Add("Please enter a valid month to show expenses", Severity.Warning);
            return;
        }

        await LoadDataForMonth(_expensesMonth.Value.Year, _expensesMonth.Value.Month);
        await _chart.RenderAsync();
    }
}
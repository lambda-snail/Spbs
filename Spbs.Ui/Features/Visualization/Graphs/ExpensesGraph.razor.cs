using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
#pragma warning disable CS8618
    [Inject] private IExpenseBatchReader _expenseReader { get; set; }
    [Inject] IJSRuntime _jSRuntime { get; set; }
    
    private GraphComponent<ExpenseVisualizationModel> _graph1;
#pragma warning restore CS8618

    private List<ExpenseVisualizationModel> _expenses = new();
    private static readonly string _noCategoryLabel = "Unassigned";

    private GraphDataFilter _filter = new() { FromDate = DateTime.Now };

    protected override async Task OnInitializedAsync()
    {
        await LoadDataForMonth(2023, 6);
    }

    private async Task LoadDataForMonth(int year, int month)
    {
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserForMonth(userId!.Value, new DateOnly(year, month, 01));
        EnsureExpensesAreCategorized();
    }

    private void EnsureExpensesAreCategorized()
    {
        foreach (var expense in _expenses)
        {
            if (string.IsNullOrWhiteSpace(expense.Category))
            {
                expense.Category = _noCategoryLabel;
            }
        }
    }

    private async Task LoadDataForDates(DateTime fromDate, DateTime? toDate)
    {
        Guid? userId = await UserId();
        _expenses = await _expenseReader.GetAllExpensesByUserBetweenDates(userId!.Value, fromDate, toDate);
        EnsureExpensesAreCategorized();
    }

    private async Task RefreshData()
    {
        await LoadDataForDates(_filter.FromDate, _filter.ToDate);
        await _graph1.Refresh();
    }
}
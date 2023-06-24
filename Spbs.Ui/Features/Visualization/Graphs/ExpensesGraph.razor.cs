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
#pragma warning disable CS8618
    [Inject] private IExpenseBatchReader _expenseReader { get; set; }
    [Inject] IJSRuntime _jSRuntime { get; set; }
#pragma warning restore CS8618

    private List<ExpenseVisualizationModel> _expenses = new();
    private static readonly string _noCategoryLabel = "Unassigned";
    
    protected override async Task OnInitializedAsync()
    {
        await LoadDataForMonth(2023, 6);
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
}
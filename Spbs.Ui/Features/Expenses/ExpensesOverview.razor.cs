using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Auth;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpensesOverview : ComponentBase
{
    private DateTime _filterLowerBound;
    private bool _filterDateHasChanged;
    private List<Expense> _cachedExpenses = new();
    
#pragma warning disable CS8618
    private Grid<Expense> _expensesGrid;
    private EditExpenseComponent _editExpenseComponent;
    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }
#pragma warning restore CS8618

    protected override void OnInitialized()
    {
        _filterLowerBound = DateTime.UtcNow;
        _filterDateHasChanged = true;
    }

    private async Task<GridDataProviderResult<Expense>> FetchExpenses(GridDataProviderRequest<Expense> request)
    {
        var filters = _expensesGrid?.GetFilters();
        if (filters is not null)
        {
            foreach (var f in filters)
            {
                if (f.PropertyName == "Date")
                {
                    DateTime newFilterCandidate = DateTime.Parse(f.Value);
                    if (newFilterCandidate != _filterLowerBound)
                    {
                        _filterLowerBound = newFilterCandidate;
                        _filterDateHasChanged = true;
                    }
                }
            }            
        }

        if (_filterDateHasChanged)
        {
            Guid? userId = await UserId();
            _cachedExpenses = await ExpenseRepository.GetSingleExpensesByUserAndMonth(userId!.Value, _filterLowerBound);
            _filterDateHasChanged = false;
        }
        
        return request.ApplyTo(_cachedExpenses);
    }

    public string GetExpenseDetailsUrl(Expense e)
    {
        return $"expenses/{e.Id}";
    }

    private async void ExpenseItemAdded()
    {
        //await FetchExpenses();
        //StateHasChanged();
    }
    
    private void ToggleExpenseDialog()
    {
        // Expense? e = null;
        // int? selectedRow = GetSelected();
        // if (selectedRow is not null && selectedRow >= 0 && selectedRow < _expenses?.Count)
        // {
        //     e = _expenses?[selectedRow.Value];
        // }
        //
        // _editExpenseComponent?.SetModalContent(e);
        // _editExpenseComponent?.ShowModal();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Auth;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseFilter 
{
    public DateTime? FromDate { get; set; }
    public bool FromDateMonthOnly { get; set; } = false;
}

[AuthenticationTaskExtension()]
public partial class ExpensesOverview : SelectableListComponent<Expense>
{
    private List<Expense>? _expenses = null;
    protected override List<Expense>? GetList() => _expenses;
    
    private bool _displayFilter = false;
    
    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }

    private ExpenseFilter _expenseFilter { get; set; } = new ExpenseFilter();
    
    private EditExpenseComponent _editExpenseComponent;
    
    public ExpensesOverview()
    {
        
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchExpenses();
    }

    private async Task FetchExpenses()
    {
        Guid? userId = await UserId();
        _expenses ??= new();
        if (_expenseFilter is { FromDate: not null })
        {
            _expenses = await ExpenseRepository.GetSingleExpensesByUserAndMonth(userId.Value, _expenseFilter.FromDate.Value);
        }
        else
        {
            _expenses = await ExpenseRepository.GetSingleExpensesByUser(userId.Value);
        }
    }

    public string GetExpenseDetailsUrl(Expense e)
    {
        return $"expenses/{e.Id}";
    }

    private async void ExpenseItemAdded()
    {
        await FetchExpenses();
        StateHasChanged();
    }
    
    private void ToggleExpenseDialog()
    {
        Expense? e = null;
        int? selectedRow = GetSelected();
        if (selectedRow is not null && selectedRow >= 0 && selectedRow < _expenses?.Count)
        {
            e = _expenses?[selectedRow.Value];
        }
        
        _editExpenseComponent?.SetModalContent(e);
        _editExpenseComponent?.ShowModal();
    }

    private string GetRowClass(int i)
    {
        return GetSelected() == i ? "bg-secondary text-white" : string.Empty;
    }

    private async void ApplyFilter()
    {
        await FetchExpenses();
        StateHasChanged();
    }
}

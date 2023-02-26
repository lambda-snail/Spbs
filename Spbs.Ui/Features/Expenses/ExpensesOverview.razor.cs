using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Spbs.Ui.Auth;
using Spbs.Ui.Components;

namespace Spbs.Ui.Features.Expenses;

public class ExpenseFilter 
{
    public DateTime? FromDate { get; set; }
    public bool FromDateMonthOnly { get; set; } = false;
}

public partial class ExpensesOverview : SelectableListComponent<Expense>
{
    private List<Expense>? _expenses = null;
    protected override List<Expense>? GetList() => _expenses;
    
    private bool _displayFilter = false;
    
    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }
    
    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; }
    
    private ExpenseFilter _expenseFilter { get; set; } = new ExpenseFilter();
    
    private EditExpenseComponent _editExpenseComponent;
    
    public ExpensesOverview()
    {
        
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchExpenses();
        await UserId();
    }

    private async Task FetchExpenses()
    {
        var authState = await authenticationStateTask;
        var user = authState.User;
        
        Guid? userId = user.GetUserId();
        if (userId is null)
        {
            return;
        }
        
        _expenses ??= new();
        // _expenses = await ExpenseRepository.GetSingleExpensesByUserAndMonth(userId.Value,
        //     new DateTime(ExpenseFilter.Year, ExpenseFilter.Month, 2));
        if (_expenseFilter?.FromDate is not null)
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
    
    private Guid? _cachedUserId = null;
    private async Task<Guid?> UserId()
    {
        if (_cachedUserId is not null)
        {
            return _cachedUserId;
        }
        
        var authState = await authenticationStateTask;
        var user = authState.User;

        _cachedUserId = user.GetUserId();
        return _cachedUserId;
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

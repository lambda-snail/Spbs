using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpensesOverview : ComponentBase
{
    private DateTime? _dateFilterYearMonth;
    private List<Expense> _cachedExpenses = new();
    
#pragma warning disable CS8618
    private EditExpenseComponent _editExpenseComponent;
    [Inject] public IExpenseReaderRepository ExpenseRepository { get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        var now = DateTime.UtcNow;
        _dateFilterYearMonth = new DateTime(now.Year, now.Month, 1);

        await ReloadData(_dateFilterYearMonth);
    }

    private async Task ReloadData(DateTime? date)
    {
        if (date is null)
        {
            _cachedExpenses.Clear();
            return;
        }
        
        Guid? userId = await UserId();
        _cachedExpenses = await ExpenseRepository.GetMonthlyExpensesForUser(userId!.Value, date.Value);
    }

    public string GetExpenseDetailsUrl(Expense e)
    {
        return $"expenses/{e.Id}";
    }

    private async void ExpenseItemAdded()
    {

        StateHasChanged();
    }
    
    private void ToggleExpenseDialog()
    {
        _editExpenseComponent?.SetModalContent(null);
        _editExpenseComponent?.ShowModal();
    }
}

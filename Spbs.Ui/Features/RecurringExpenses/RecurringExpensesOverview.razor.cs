using System;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class RecurringExpensesOverview
{
    private RecurringExpensesListComponent _recurringExpensesList;
    private RecurringExpense? _selectedExpense = null; 
    
    private void SetFocusRecurrenceType(RecurrenceType? type)
    {
        _recurringExpensesList.SetFilter(new()
        {
            RecurrenceType = type
        });
    }
}
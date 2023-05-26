#nullable enable

using System;
using System.Collections.Generic;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses;

/// <summary>
/// Helps components related to loading and importing of expenses to keep track of the process and to
/// communicate with each other.
/// </summary>
public class ImportExpensesStateManager
{
    public event EventHandler? NumExpensesImportedChanged;
    public void NotifyExpenseImported()
    {
        NumExpensesImportedChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Resets the state.
    /// </summary>
    public void ImportJobComplete()
    {
        _expensesToConfigure.Clear();
        _expensesToImport.Clear();
    }
    
    public List<ImportExpensesViewModel> _expensesToConfigure { get; set; } = new();

    public List<Expense> _expensesToImport { get; set; } = new();
}
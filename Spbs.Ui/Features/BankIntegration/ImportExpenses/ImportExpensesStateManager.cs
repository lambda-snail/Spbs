#nullable enable

using System.Collections.Generic;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses;

/// <summary>
/// Helps components related to loading and importing of expenses to keep track of the process and to
/// communicate with each other.
/// </summary>
public class ImportExpensesStateManager
{
    public enum ImportExpensesState
    {
        NotStarted,
        ExpensesLoadedFromApi,
        ConfiguringImport,
        Importing
    }

    public List<ImportExpensesViewModel> _expensesToImport { get; set; } = new();
    
    
}
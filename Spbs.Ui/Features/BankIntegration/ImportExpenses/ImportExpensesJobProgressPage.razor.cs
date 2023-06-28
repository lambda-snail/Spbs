using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses;

[AuthenticationTaskExtension]
public partial class ImportExpensesJobProgressPage : ComponentBase
{
    private bool _noJob = true;
    private bool _isImportJobComplete = false;
    private bool _isImportingExpenses = false;
    private int _numExpensesToImport = 0;
    private int _numExpensesImported = 0;

    [Inject] private ImportExpensesStateManager _importState { get; set; }
    [Inject] private IExpenseWriterRepository _expenseRepository { get; set; }
    
    protected override void OnInitialized()
        => _importState.NumExpensesImportedChanged += this.SingleExpenseImportComplete;
    
    public void Dispose()
        => _importState.NumExpensesImportedChanged -= this.SingleExpenseImportComplete;
    
    protected override async Task OnInitializedAsync()
    {
        var expenses = _importState._expensesToImport;
        if (expenses is { Count: >0 })
        {
            _isImportingExpenses = true;
            _isImportJobComplete = false;

            _numExpensesToImport = expenses.Count;
            _numExpensesImported = 0;

            Guid? userId = await UserId();
            if (userId is null)
            {
                return;
            }

            foreach (var expense in expenses)
            {
                expense.UserId = userId.Value;
                await _expenseRepository.InsertExpenseAsync(expense);
                _importState.NotifyExpenseImported();
            }

            _isImportingExpenses = false;
            _isImportJobComplete = true;
            _importState.ImportJobComplete();
        }
    }

    private void SingleExpenseImportComplete(object? obj, EventArgs args)
    {
        _numExpensesImported += 1;
        StateHasChanged();
    }
}
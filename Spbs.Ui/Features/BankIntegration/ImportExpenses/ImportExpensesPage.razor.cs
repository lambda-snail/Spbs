using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses;

[AuthenticationTaskExtension]
public partial class ImportExpensesPage : ComponentBase
{
    private class LoadAccountsParameters
    {
        public bool IncludeAll { get; set; } = false;
    }

    private struct ViewModelExpenseSuggestionPair
    {
        public ImportExpensesViewModel ViewModel { get; set; }
        public Expense ImportedExpense { get; set; }
    }
    
#pragma warning disable CS8618
    [Inject] private ImportExpensesStateManager _importState { get; set; }
    [Inject] private IMapper _mapper { get; set; }
#pragma warning restore CS8618

    private List<ViewModelExpenseSuggestionPair> _viewModelsWithGeneratedExpenses = new();
    private List<ImportExpensesViewModel> _filteredExpenseModels = new(); 
    
    private LoadAccountsParameters _loadAccountsParameters = new();
    
    private Expense? _expenseToEdit;

    protected override async Task OnInitializedAsync()
    {
        _userId = await UserId();
        
        if (_importState._expensesToImport is { Count: >0 })
        {
            _filteredExpenseModels = _importState._expensesToImport.Where(vm => vm.TransactionAmount.Amount < 0d).ToList();
            foreach (var importExpensesViewModel in _filteredExpenseModels)
            {
                importExpensesViewModel.TransactionAmount.Amount *= -1d;
            }
            
            ReloadExpenseMappings();
        }
    }

    private void ReloadExpenseMappings()
    {
        _viewModelsWithGeneratedExpenses.Clear();
        foreach (var importExpensesViewModel in _filteredExpenseModels)
        {
            _viewModelsWithGeneratedExpenses.Add(new()
            {
                ViewModel = importExpensesViewModel,
                ImportedExpense = _mapper.Map<Expense>(importExpensesViewModel) 
            });
        }
    }

    private void ToggleIncludeAll()
    {
        _loadAccountsParameters.IncludeAll = !_loadAccountsParameters.IncludeAll;
        foreach (var importExpensesViewModel in _filteredExpenseModels)
        {
            importExpensesViewModel.IncludeInImport = _loadAccountsParameters.IncludeAll;
        }
        
        StateHasChanged();
    }

    private void HandleInvalidSubmit_EditExpense()
    {
        
    }

    private void HandleValidSubmit_EditExpense()
    {
        _expenseToEdit = null;
    }
}
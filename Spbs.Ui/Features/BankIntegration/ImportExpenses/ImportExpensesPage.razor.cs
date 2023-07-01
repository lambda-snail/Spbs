using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Ui.Features.BankIntegration.Services;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.BankIntegration.ImportExpenses;

public partial class ImportExpensesPage : ComponentBase
{
#pragma warning disable CS8618
    [Inject] private ImportExpensesStateManager _importState { get; set; }
    [Inject] private IRedirectLinkService _redirectService { get; set; }
    [Inject] private NavigationManager _navigationManager { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    
    private MudDataGrid<ImportExpensesViewModel> _grid;
#pragma warning restore CS8618
    
    private List<ImportExpensesViewModel> _filteredExpenseModels = new(); 
    
    private HashSet<ImportExpensesViewModel> _selectedTransactions = new();
    private bool _includeAllSwitchValue = true;
    
    protected override void OnInitialized()
    {
        if (_importState._expensesToConfigure is { Count: >0 })
        {
            _filteredExpenseModels = _importState._expensesToConfigure.Where(vm => vm.TransactionAmount.Amount < 0d).ToList();
            foreach (var importExpensesViewModel in _filteredExpenseModels)
            {
                importExpensesViewModel.TransactionAmount.Amount *= -1d;
            }
        }
    }

    private void ToggleInclude(IEnumerable<ImportExpensesViewModel> itemsToToggle)
    {
        foreach (var importExpensesViewModel in itemsToToggle)
        {
            importExpensesViewModel.IncludeInImport = _includeAllSwitchValue;
        }
        
        StateHasChanged();
    }

    private void ImportExpenses()
    {
        var expenses = _mapper.Map<List<Expense>>(_filteredExpenseModels.Where(t => t.IncludeInImport).ToList());
        _importState._expensesToImport = expenses;
        _navigationManager.NavigateTo(_redirectService.GetUrlForImportExpenses(isInProgressPage: true));
    }

    private void OnSelectionChanged(HashSet<ImportExpensesViewModel> selection)
    {
        _selectedTransactions = selection;
    }

    private void OnTransactionEdited(ImportExpensesViewModel obj)
    {
        
    }
}
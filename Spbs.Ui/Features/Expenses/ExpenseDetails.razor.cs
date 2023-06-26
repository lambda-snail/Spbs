using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpenseDetails : ComponentBase
{
    [Parameter] public string ExpenseId { get; set; }
    private Expense? _expense = null;
    
#pragma warning disable CS8618
    [Inject] private IExpenseReaderRepository _expenseReaderRepository { get; set; }
    [Inject] private IExpenseWriterRepository _expenseWriterRepository { get; set; }
    [Inject] private IJSRuntime _jsRuntime { get; set; }
    [Inject] private IMapper _mapper { get; set; }
    
    [Inject] ISnackbar _snackbar { get; set; }
    
    private EditExpenseComponent _editExpenseComponent;
    private EditExpenseItemComponent _editExpenseItemComponent;
    private MudDataGrid<ExpenseItem> _grid;
#pragma warning restore CS8618
    
    protected override void OnInitialized()
    {
        FetchExpense();
    }

    private async void FetchExpense()
    {
        Guid id = Guid.Parse(ExpenseId);
        Guid? userId = await UserId();
        _expense = await _expenseReaderRepository.GetUserExpenseById(id, userId!.Value);
        StateHasChanged();
    }

    private async Task SaveExpense()
    {
        if (_expense is not null)
        {
            await _expenseWriterRepository.UpdateExpenseAsync(_expense);            
        }

        _snackbar.Add("Changes saved successfully!", Severity.Success);
        StateHasChanged();
    }

    private async Task AddTagList()
    {
        string tagList = await _jsRuntime.InvokeAsync<string>("prompt", "Add a list of tags separated by space");
        if (string.IsNullOrWhiteSpace(tagList))
        {
            return;
        }

        _expense!.Tags ??= string.Empty;

        _expense!.Tags += " " + tagList;
        await SaveExpense();
    }

    private void ExpenseUpdated()
    {
        FetchExpense();
    }
    
    private void ToggleEditMode()
    {
        _editExpenseComponent?.SetModalContent(_expense);
        _editExpenseComponent?.ShowModal();
    }

    private void AddExpenseItem()
    {
        var newItem = new ExpenseItem { Id = Guid.NewGuid() };
        
        _expense!.Items.Add(newItem);
        _grid.ReloadServerData();
        _grid.SetEditingItemAsync(newItem);
    }

    private Task OnExpenseItemChanged()
    {
        return SaveExpense();
    }
}
using System;
using System.Linq;
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
        if (_expense is null)
        {
            return;
        }

        await _expenseWriterRepository.UpdateExpenseAsync(_expense!);
        StateHasChanged();
    }

    private void ToggleEditMode()
    {
        _editExpenseComponent?.SetModalContent(_expense);
        _editExpenseComponent?.ShowModal();
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
    
    private async Task ExpenseItemUpdated(ExpenseItem? item)
    {
        if (item is null)
        {
            return;
        }

        ExpenseItem? existingItem = _expense!.Items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem is null)
        {
            _expense.Items.Add(item);
        }
        
        await SaveExpense();
    }
    
    private void EditOrCreateExpenseItem(ExpenseItem item)
    {
        _editExpenseItemComponent?.SetModalContent(item);
        _editExpenseItemComponent?.ShowModal();
    }
}
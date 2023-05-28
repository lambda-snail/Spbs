using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Spbs.Generators.UserExtensions;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension()]
public partial class ExpenseDetails : ComponentBase
{
    [Parameter] public string ExpenseId { get; set; }

    [Inject] public IExpenseReaderRepository ExpenseReaderRepository { get; set; }
    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public IMapper Mapper { get; set; }

    private Expense? _expense = null;
    private EditExpenseComponent _editExpenseComponent;
    private EditExpenseItemComponent _editExpenseItemComponent;

    protected override void OnInitialized()
    {
        FetchExpense();
    }

    private async void FetchExpense()
    {
        Guid id = Guid.Parse(ExpenseId);
        Guid? userId = await UserId();
        _expense = await ExpenseReaderRepository.GetUserExpenseById(id, userId!.Value);
        StateHasChanged();
    }

    private async Task SaveExpense()
    {
        if (_expense is null)
        {
            return;
        }

        await ExpenseWriterRepository.UpdateExpenseAsync(_expense!);
        StateHasChanged();
    }

    private void ToggleEditMode()
    {
        _editExpenseComponent?.SetModalContent(_expense);
        _editExpenseComponent?.ShowModal();
    }

    private async Task AddTagList()
    {
        string tagList = await JsRuntime.InvokeAsync<string>("prompt", "Add a list of tags separated by space");
        if (tagList is null || tagList == string.Empty)
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
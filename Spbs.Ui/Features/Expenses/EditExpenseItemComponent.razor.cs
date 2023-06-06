using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.Expenses;

public partial class EditExpenseItemComponent : ComponentBase
{
    private bool _doShowContent = false;
    
    private EditExpenseItemViewModel _editExpenseItemViewModel = new();
    private ExpenseItem? _item;
    
    [Inject] public IMapper mapper { get; set; }
    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; } 
    
    [Parameter, Required] public Func<Guid?> GetUserId { get; set; }
    
    /// <summary>
    /// Callback used when an expense item has been updated. If a new item has been created or modified, it will be returned here.
    /// </summary>
    [Parameter] public Func<ExpenseItem?, Task> OnUpdateCallback { get; set; }
    
    public void ShowModal()
    {
        _doShowContent = true;
        StateHasChanged();
    }

    private void CloseDialog()
    {
        _doShowContent = false;
        StateHasChanged();
    }
    
    /// <summary>
    /// To edit an Expense Item using this component, use this method to set the content before displaying the component.
    /// </summary>
    public void SetModalContent(ExpenseItem? expenseItem = null)
    {
        if (expenseItem is not null)
        {
            _item = expenseItem;
            _editExpenseItemViewModel = mapper.Map<EditExpenseItemViewModel>(expenseItem);
        }
        else
        {
            ResetModel();
        }
    }

    private async Task HandleValidSubmit()
    {
        CloseDialog();

        ExpenseItem item;
        if (_item is null)
        {
            item = mapper.Map<ExpenseItem>(_editExpenseItemViewModel);
            item.Id = Guid.NewGuid();
        }
        else
        {
            item = mapper.Map(_editExpenseItemViewModel, _item);
        }
        
        await OnUpdateCallback(item);
        
        ResetModel();
        StateHasChanged();
    }

    private void HandleInvalidSubmit()
    {
        
    }

    private void ResetModel()
    {
        _item = null;
        _editExpenseItemViewModel = new();
    }
}
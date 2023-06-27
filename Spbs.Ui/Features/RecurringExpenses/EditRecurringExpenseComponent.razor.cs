using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class EditRecurringExpenseComponent
{
    private bool _doShowContent = false;

    private EditRecurringExpenseViewModel _editRecurringExpenseViewModel = new() { BillingDay = DateTime.Now.Day };
    private RecurringExpense? _expense;


#pragma warning disable CS8618
    [Inject] public IMapper Mapper { get; set; }
    [Inject] public IRecurringExpenseWriterRepository RecurringExpenseWriterRepository { get; set; } 
    
    [Parameter, Required] public Func<Guid?> GetUserId { get; set; }
    [Parameter] public Func<RecurringExpense, Task> OnUpdateCallback { get; set; }
#pragma warning restore CS8618
    
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
    /// To edit a Recurring Expense using this component, use this method to set the content before displaying the component.
    /// </summary>
    public void SetModalContent(RecurringExpense? expense = null)
    {
        if (expense is not null)
        {
            _expense = expense;
            _editRecurringExpenseViewModel = Mapper.Map<EditRecurringExpenseViewModel>(expense);
        }
    }

    private async Task HandleValidSubmit()
    {
        RecurringExpense expense;
        if (_expense is null)
        {
            expense = Mapper.Map<RecurringExpense>(_editRecurringExpenseViewModel);
        }
        else
        {
            expense = Mapper.Map(_editRecurringExpenseViewModel, _expense);
        }
        
        EnsureUseIdIsSet(expense);
        if (_editRecurringExpenseViewModel.Id is null)
        {
            await RecurringExpenseWriterRepository.InsertExpenseAsync(expense);
        }
        else
        {
            await RecurringExpenseWriterRepository.UpdateExpenseAsync(expense);
        }
        
        CloseDialog();
        ResetModel();
        await OnUpdateCallback(expense);
        StateHasChanged();
    }

    private void HandleInvalidSubmit()
    {
        
    }

    private void ResetModel()
    {
        _editRecurringExpenseViewModel = new() { BillingDay = DateTime.Now.Day };
    }   
    
    private void EnsureUseIdIsSet(RecurringExpense expense)
    {
        if (expense.UserId == Guid.Empty)
        {
            Guid? userId = GetUserId();
            if (userId is null)
            {
                return;
            }

            expense.UserId = userId.Value;
        }
    }
}
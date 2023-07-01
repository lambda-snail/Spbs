using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class EditRecurringExpenseComponent
{
    private bool _doShowContent = false;

    private EditRecurringExpenseViewModel _editRecurringExpenseViewModel = new() { BillingDay = DateTime.Now.Day };
    private RecurringExpense? _expense;
    
#pragma warning disable CS8618
    [Inject] private IMapper _mapper { get; set; }
    [Inject] private IRecurringExpenseWriterRepository _recurringExpenseWriterRepository { get; set; }
    [Inject] private IValidator<EditRecurringExpenseViewModel> _expenseValidator { get; set; }
    
    [Parameter] public Func<Guid?> GetUserId { get; set; }
    [Parameter] public Func<RecurringExpense, Task> OnUpdateCallback { get; set; }
    
    private MudForm _form;
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
            _editRecurringExpenseViewModel = _mapper.Map<EditRecurringExpenseViewModel>(expense);
        }
    }

    private async Task HandleValidSubmit()
    {
        RecurringExpense expense;
        if (_expense is null)
        {
            expense = _mapper.Map<RecurringExpense>(_editRecurringExpenseViewModel);
        }
        else
        {
            expense = _mapper.Map(_editRecurringExpenseViewModel, _expense);
        }
        
        EnsureUseIdIsSet(expense);
        if (_editRecurringExpenseViewModel.Id is null)
        {
            await _recurringExpenseWriterRepository.InsertExpenseAsync(expense);
        }
        else
        {
            await _recurringExpenseWriterRepository.UpdateExpenseAsync(expense);
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
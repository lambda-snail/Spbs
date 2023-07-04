using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Ui.Features.Expenses.Models;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class EditRecurringExpenseComponent
{
    private bool _doShowContent = false;

    private EditRecurringExpenseViewModel _editRecurringExpenseViewModel = new() { BillingDay = DateTime.Now.Day };
    private RecurringExpense? _expense;
    
    private IReadOnlyList<Category> _expenseCategories = ExpenseCategoryUtils.GetAllCategories();
    
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
        
        EnsureUserIdIsSet(expense);
        await _recurringExpenseWriterRepository.UpsertExpenseAsync(expense);

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
    
    private void EnsureUserIdIsSet(RecurringExpense expense)
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
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Spbs.Ui.Features.Expenses;

namespace Spbs.Ui.Features.RecurringExpenses;

public partial class EditRecurringExpenseComponent
{
    private bool _doShowContent = false;

    private EditRecurringExpenseViewModel _editRecurringExpenseViewModel = new() { BillingDate = DateTime.Now };
            

    [Inject] public IMapper Mapper { get; set; }
    [Inject] public IRecurringExpenseWriterRepository RecurringExpenseWriterRepository { get; set; } 
    
    [Parameter, Required] public Func<Guid?> GetUserId { get; set; }
    [Parameter] public Action OnUpdateCallback { get; set; }
    
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
            _editRecurringExpenseViewModel = Mapper.Map<EditRecurringExpenseViewModel>(expense);
        }
    }

    private async Task HandleValidSubmit()
    {
        RecurringExpense expense = Mapper.Map<RecurringExpense>(_editRecurringExpenseViewModel);
        
        if (_editRecurringExpenseViewModel.Id is null)
        {
            Guid? userId = GetUserId();
            if (userId is null)
            {
                return;
            }

            expense.OwningUserId = userId.Value;
            await RecurringExpenseWriterRepository.InsertExpenseAsync(expense);
        }
        else
        {
            await RecurringExpenseWriterRepository.UpdateExpenseAsync(expense);
        }
        
        CloseDialog();
        ResetModel();
        OnUpdateCallback();
        StateHasChanged();
    }

    private void HandleInvalidSubmit()
    {
        
    }

    private void ResetModel()
    {
        _editRecurringExpenseViewModel = new() { BillingDate = DateTime.Now };
    }   
}
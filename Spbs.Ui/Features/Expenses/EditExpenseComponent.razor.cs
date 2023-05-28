using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.Expenses;

public partial class EditExpenseComponent : ComponentBase
{
    private bool _doShowContent = false;

    private Expense? _expense;
    private EditExpenseViewModel _editExpenseViewModel = new() { Date = DateTime.Now };

    [Inject] public IMapper mapper { get; set; }
    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; } 
    
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
    /// To edit an Expense using this component, use this method to set the content before displaying the component.
    /// </summary>
    public void SetModalContent(Expense? expense = null)
    {
        if (expense is not null)
        {
            _expense = expense;
            _editExpenseViewModel = mapper.Map<EditExpenseViewModel>(expense);
        }
    }

    private async Task HandleValidSubmit()
    {
        Expense expense;
        if (_expense is null)
        {
            expense = mapper.Map<Expense>(_editExpenseViewModel);
        }
        else
        {
            expense = mapper.Map(_editExpenseViewModel, _expense);
        }

        EnsureUseIdIsSet(expense);
        if (_editExpenseViewModel.Id is null)
        {
            await ExpenseWriterRepository.InsertExpenseAsync(expense);
        }
        else
        {
            await ExpenseWriterRepository.UpdateExpenseAsync(expense);
        }
        
        _expense = null;
        
        CloseDialog();
        ResetModel();
        OnUpdateCallback();
        StateHasChanged();
    }

    private void EnsureUseIdIsSet(Expense expense)
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

    private void HandleInvalidSubmit()
    {
        
    }

    private void ResetModel()
    {
        _editExpenseViewModel = new() { Date = DateTime.Now };
    }
}
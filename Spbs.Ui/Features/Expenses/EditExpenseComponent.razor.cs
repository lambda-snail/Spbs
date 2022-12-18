using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.Expenses;

public partial class EditExpenseComponent : ComponentBase
{
    private bool _doShowContent = false;

    private EditExpenseViewModel _editExpenseViewModel = new() { Date = DateTime.Now };

    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; } 
    
    [Parameter, Required] public Func<Guid?> GetUserId { get; set; }
    
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
    public void SetModalContent(Expense? expense)
    {
        
    }

    private async Task HandleValidSubmit()
    {
        if (_editExpenseViewModel.Id is null)
        {
            CloseDialog();
            
            Guid? userId = GetUserId();
            if (userId is null)
            {
                return;
            }

            _editExpenseViewModel.OwningUserId = userId.Value;
            await ExpenseWriterRepository.InsertAsync(_editExpenseViewModel);
        }
        else
        {
            await ExpenseWriterRepository.UpdateAsync(_editExpenseViewModel);
        }
    }

    private void HandleInvalidSubmit()
    {
        
    }
}
using System;
using Microsoft.AspNetCore.Components;

namespace Spbs.Ui.Features.Expenses;

public partial class EditExpenseComponent : ComponentBase
{
    private bool _doShowContent = false;

    private EditExpenseViewModel _editExpenseViewModel = new() { Date = DateTime.Now };

    [Inject] public IExpenseWriterRepository ExpenseWriterRepository { get; set; } 
    
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

    private void HandleValidSubmit()
    {
        if (_editExpenseViewModel.Id is null)
        {
            Expense e = new() { Name = };
            ExpenseWriterRepository.InsertAsync();
        }
    }

    private void HandleInvalidSubmit()
    {
        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spbs.Generators.UserExtensions;
using Spbs.Ui.Features.Users;
using Spbs.Ui.Features.Users.Repositories;
using Shared.Utilities;
using Severity = MudBlazor.Severity;

namespace Spbs.Ui.Features.Expenses;

[AuthenticationTaskExtension]
public partial class EditExpenseComponent : ComponentBase
{
    private bool _doShowContent = false;

    private Expense? _expense;
    private EditExpenseViewModel _editExpenseViewModel = new() { Date = DateTime.Now };

    private List<string> _expenseCategories = new();

#pragma warning disable CS8618
    [Inject] private IMapper _mapper { get; set; }
    [Inject] private IExpenseWriterRepository _expenseWriterRepository { get; set; } 
    [Inject] private IUserRepository _userRepository { get; set; }
    [Inject] private IValidator<EditExpenseViewModel> _expenseValidator { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }

    [Parameter, Required] public Func<Guid?> GetUserId { get; set; }
    [Parameter] public Action OnUpdateCallback { get; set; }
    
    private MudForm _form;
#pragma warning restore CS8618
    
    public void ShowModal()
    {
        _doShowContent = true;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        Guid? userId = await UserId();
        User? user = await _userRepository.GetById(_userId!.Value);
        if (user is null)
        {
            return;
        }

        _expenseCategories = user.ExpenseCategories;
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
            _editExpenseViewModel = _mapper.Map<EditExpenseViewModel>(expense);
        }
    }

    private async Task HandleValidSubmit()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            _snackbar.Add("Changes saved!");
        }
        else
        {
            _snackbar.Add("An error occured while saving", Severity.Error);
        }

        CloseDialog();
        
        return;
        
        Expense expense;
        if (_expense is null)
        {
            expense = _mapper.Map<Expense>(_editExpenseViewModel);
        }
        else
        {
            expense = _mapper.Map(_editExpenseViewModel, _expense);
        }

        EnsureUseIdIsSet(expense);
        if (_editExpenseViewModel.Id is null)
        {
            await _expenseWriterRepository.InsertExpenseAsync(expense);
        }
        else
        {
            await _expenseWriterRepository.UpdateExpenseAsync(expense);
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

    /// <summary>
    /// Update the date of the view model whenever there is a change. This is a workaround since mud blazor date picker
    /// seems to be unable to bind to a DateTime - it only works with a nullable DateTime.
    /// </summary>
    private void OnDatePickerDateChanged(DateTime? newDate)
    {
        if(newDate is null) return;
        _editExpenseViewModel.Date = newDate.Value;

    }
}
using FluentValidation;

namespace Spbs.Ui.Features.Expenses.Validation;

public class EditExpenseViewModelFluentValidation : AbstractValidator<EditExpenseViewModel>
{
    public EditExpenseViewModelFluentValidation()
    {
        RuleFor(e => e.Name).NotEmpty().WithMessage("Please provide a name for the expense");
        RuleFor(e => e.Date).NotEmpty().NotNull().WithMessage("Please provide a date for the expense");
        RuleFor(e => e.Currency).NotEmpty().WithMessage("Please provide a currency for the expense");
        RuleFor(e => e.Total).NotNull().GreaterThan(0).WithMessage("The total cost must be positive");
    }
}
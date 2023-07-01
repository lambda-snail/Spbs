using FluentValidation;

namespace Spbs.Ui.Features.RecurringExpenses.Validation;

public class EditRecurringExpenseViewModelFluentValidation : AbstractValidator<EditRecurringExpenseViewModel>
{
    public EditRecurringExpenseViewModelFluentValidation()
    {
        RuleFor(e => e.Name).NotEmpty().WithMessage("Please provide a name for the expense");
        RuleFor(e => e.Description).MaximumLength(4000)
            .WithMessage("Description too long. The description is capped at 4,000 characters!");
        RuleFor(e => e.BillingDay).InclusiveBetween(1, 31).WithMessage("The billing day must be a day of the month");
        RuleFor(e => e.BillingPrincipal).NotEmpty().WithMessage("Please provide the name of the recipient");
        RuleFor(e => e.Total).GreaterThan(0).WithMessage("The cost must not be greater than 0");
        RuleFor(e => e.Currency).NotEmpty().WithMessage("Please provide a currency for the expense");
    }
}
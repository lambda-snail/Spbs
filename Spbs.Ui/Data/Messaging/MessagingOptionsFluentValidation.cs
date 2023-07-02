using FluentValidation;

namespace Spbs.Ui.Data.Messaging;

public class MessagingOptionsFluentValidation : AbstractValidator<MessagingOptions>
{
    public MessagingOptionsFluentValidation()
    {
        RuleFor(o => o.RecurringExpensesQueue).NotEmpty();
    }
}
using System;
using FluentValidation;

namespace Spbs.Ui.Features.BankIntegration.Models.Validation;

public class TransactionsParametersRequestFluentValidation : AbstractValidator<TransactionsRequestParameters>
{
    public TransactionsParametersRequestFluentValidation()
    {
        When(p => p.Range.Start is not null, () =>
        {
            RuleFor(p => p.Range.Start).GreaterThan(new DateTime(1970, 01, 01)).WithMessage("Please select a from date on a later date");
            
            RuleFor(p => p.Range.End).NotEmpty().GreaterThan(new DateTime(1970, 01, 01)).WithMessage("Please select a to date on a later date");
            RuleFor(p => p.Range.End).GreaterThanOrEqualTo(p => p.Range.Start).WithMessage("The to date must be after the from date");
        });
    }
}
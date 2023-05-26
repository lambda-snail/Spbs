using System;
using FluentValidation;

namespace Spbs.Ui.Features.BankIntegration.Models.Validation;

public class TransactionsParametersRequestFluentValidation : AbstractValidator<TransactionsRequestParameters>
{
    public TransactionsParametersRequestFluentValidation()
    {
        When(p => p.DateFrom is not null, () =>
        {
            RuleFor(p => p.DateFrom).GreaterThan(new DateOnly(1970, 01, 01)).WithMessage("Please select a from date on a later date");
            
            RuleFor(p => p.DateTo).GreaterThan(new DateOnly(1970, 01, 01)).WithMessage("Please select a to date on a later date");
            RuleFor(p => p.DateTo).GreaterThanOrEqualTo(p => p.DateFrom).WithMessage("The to date must be after the from date");
        });
        
        When(p => p.DateFrom is null, () =>
        {
            RuleFor(p => p.DateTo).Empty().WithMessage("To date must be empty if there is no from date");
        });
    }
}
using FluentValidation;

namespace Spbs.Ui.Features.BankIntegration.Models.Validation;

public class NordigenEulaFluentValidation : AbstractValidator<NordigenEula>
{
    public NordigenEulaFluentValidation()
    {
        RuleFor(e => e.AccessScope)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.AccessValidForDays).GreaterThanOrEqualTo(0).LessThanOrEqualTo(180); // Arbitrary upper limit
        RuleFor(e => e.MaxHistoricalDays).GreaterThanOrEqualTo(0).LessThanOrEqualTo(180); // Arbitrary upper limit
        RuleFor(e => e.InstitutionId).NotEmpty();
        RuleFor(e => e.UserId).NotEmpty();
        RuleFor(e => e.Created).NotEmpty();
        RuleFor(e => e.InstitutionId).NotEmpty();
    }
}
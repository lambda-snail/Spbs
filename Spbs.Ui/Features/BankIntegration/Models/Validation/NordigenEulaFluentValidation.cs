using FluentValidation;

namespace Spbs.Ui.Features.BankIntegration.Models.Validation;

public class NordigenEulaFluentValidation : AbstractValidator<NordigenEula>
{
    public NordigenEulaFluentValidation()
    {
        RuleFor(e => e.AccessScope).NotEmpty();

        RuleFor(e => e.AccessValidForDays).GreaterThanOrEqualTo(1).LessThanOrEqualTo(90); // Values from Nordigen
        RuleFor(e => e.MaxHistoricalDays).GreaterThanOrEqualTo(1).LessThanOrEqualTo(730);
        RuleFor(e => e.InstitutionId).NotEmpty();
        RuleFor(e => e.UserId).NotEmpty();
        RuleFor(e => e.Created).NotEmpty();
        RuleFor(e => e.InstitutionId).NotEmpty();
    }
}
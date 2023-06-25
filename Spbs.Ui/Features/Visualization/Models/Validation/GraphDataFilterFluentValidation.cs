using FluentValidation;

namespace Spbs.Ui.Features.Visualization.Models.Validation;

public class GraphDataFilterFluentValidation : AbstractValidator<GraphDataFilter>
{
    public GraphDataFilterFluentValidation()
    {
        When(f => f.ToDate is not null, () =>
        {
            RuleFor(f => f.FromDate)
                .LessThan(f => f.ToDate)
                .WithMessage("The from date must be before the to date.");
        });
    }
}
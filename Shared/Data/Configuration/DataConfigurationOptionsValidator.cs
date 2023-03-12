using FluentValidation;

namespace Spbs.Shared.Data;

public class DataConfigurationOptionsValidator : AbstractValidator<DataConfigurationOptions>
{
    public DataConfigurationOptionsValidator()
    {
        RuleFor(o => o.DatabaseName).NotNull().NotEmpty();
        RuleFor(o => o.DataContainerName).NotNull().NotEmpty();
    }
}

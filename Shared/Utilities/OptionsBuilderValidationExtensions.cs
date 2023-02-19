using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Shared.Utilities.OptionsExtensions;

/// <summary>
/// Adds fluent validation of IOptions on startup. Credits go to Nick Chapsas (https://www.youtube.com/watch?v=jblRYDMTtvg).
/// </summary>
public static class OptionsBuilderValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services
            .AddSingleton<IValidateOptions<TOptions>>(s => new FluentValidationOptions<TOptions>(optionsBuilder.Name, s.GetService<IValidator<TOptions>>()));
        return optionsBuilder;
    }
}

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;
    public string? Name { get; }
    
    public FluentValidationOptions(string? name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }

    public ValidateOptionsResult Validate(string name, TOptions options)
    {
        if (Name != null && Name != name)
        {
            return ValidateOptionsResult.Skip;
        }
        
        ArgumentNullException.ThrowIfNull(options);
        
        var validationResults = _validator.Validate(options);
        if (validationResults.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var errors = validationResults.Errors.Select(e =>
            $"Options validation failed for ${e.PropertyName} with message ${e.ErrorMessage}");
        
        return ValidateOptionsResult.Fail(errors);
    }
}
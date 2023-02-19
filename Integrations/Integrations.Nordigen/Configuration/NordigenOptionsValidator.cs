using FluentValidation;

namespace Integrations.Nordigen;

public class NordigenOptionsValidator : AbstractValidator<NordigenOptions>
{
    public NordigenOptionsValidator()
    {
        RuleFor(o => o.ClientId).NotEmpty().NotNull();
        RuleFor(o => o.ClientSecret).NotEmpty().NotNull();
        RuleFor(o => o.ServiceUrl).NotEmpty().NotNull();
        RuleFor(o => o.CallbackUrl).NotEmpty().NotNull();

        RuleFor(o => o.ServiceUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
        RuleFor(o => o.CallbackUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
        RuleFor(o => o.NewTokenEndpoint)
            .NotEmpty()
            .NotNull()
            .Must(uri => uri.EndsWith('/')) // Post redirects to Get otherwise
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
        RuleFor(o => o.RefreshTokenEndpoint)
            .NotEmpty()
            .NotNull()
            .Must(uri => uri.EndsWith('/'))
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
        
        RuleFor(o => o.DefaultMaxHistoricalDays)
            .GreaterThan(0)
            .When(o => o.DefaultMaxHistoricalDays is not null);
        RuleFor(o => o.DefaultAccessValidForDays)
            .GreaterThan(0)
            .When(o => o.DefaultAccessValidForDays is not null);
    }
}
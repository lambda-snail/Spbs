using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utilities.OptionsExtensions;

namespace Integrations.Nordigen;

public static class Startup
{
    public static void RegisterNordigenIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

        services.AddSingleton<IValidator<NordigenOptions>, NordigenOptionsValidator>();
        
        services.AddOptions<NordigenOptions>()
            .BindConfiguration(NordigenOptions.NordigenOptionsSectionName)
            .ValidateFluently()
            .ValidateOnStart();

        services.AddHttpClient<NordigenTokenClient>();
        services.AddHttpClient<INordigenApiClient, NordigenApiClient>();

        services.AddLazyCache();
        
        services.AddAutoMapper(typeof(Integrations.Nordigen.Startup));
    }
}
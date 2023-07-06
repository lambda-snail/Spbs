using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Spbs.Ui;

public class Program
{
    public static void Main(string[] args)
    {
        ConfigureLoggingForStartup();

        try
        {
            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Host terminated unexpectedly");
            return;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        var tempConfiguration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (env == "Production")
        {
            var serilogConfig = tempConfiguration.GetSection("Serilog");

            // APPINSIGHTS_INSTRUMENTATIONKEY
            builder.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .ReadFrom.Configuration(serilogConfig)
                // .WriteTo.ApplicationInsights(
                //     services.GetRequiredService<TelemetryConfiguration>(),
                //     TelemetryConverter.Traces)
                .Enrich.FromLogContext());
        }
        else
        {
            builder.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext());
        }

        TokenCredential credential = CreateCredential(env, tempConfiguration);

        builder.ConfigureWebHostDefaults(webBuilder =>
            webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //var settings = config.Build();
                    var appConfigEndpoint =
                        tempConfiguration.GetSection("AppConfigBootstrap").GetValue<string>("Endpoint");
                    ArgumentNullException.ThrowIfNull(appConfigEndpoint);

                    //var refreshTimer = settings.GetSection("AppConfigBootstrap").GetValue<int?>("DefaultConfigRefreshHours");
                    config.AddAzureAppConfiguration(options =>
                            options
                                .Connect(new Uri(appConfigEndpoint),
                                    credential) // or ManagedIdentityCredential?
                                .Select("Spbs:*", LabelFilter.Null)
                                .Select("Spbs:*", env)
                        //.ConfigureRefresh(refreshOptions => refreshOptions.SetCacheExpiration(TimeSpan.FromHours(refreshTimer ?? 24)))
                    );

                    if (env == "Development")
                    {
                        config.AddUserSecrets<Spbs.Ui.Program>();
                    }

                    config.Build();
                })
                .UseStartup<Startup>(context =>
                {
                    return new Startup(context.Configuration, credential);
                }));

        return builder;
    }

    private static void ConfigureLoggingForStartup()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    private static TokenCredential CreateCredential(string env, IConfiguration config)
    {
        if (env == "Development")
        {
            var tenantId = config.GetSection("LocalDevelopmentCredentials")
                .GetValue<string>("TenantId");
            var clientId = config.GetSection("LocalDevelopmentCredentials")
                .GetValue<string>("ClientId");
            var clientSecret = config.GetSection("LocalDevelopmentCredentials")
                .GetValue<string>("ClientSecret");
            return new ClientSecretCredential(tenantId, clientId, clientSecret);
        }

        return new ManagedIdentityCredential();
    }
}

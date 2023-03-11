using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Spbs.Ui
{
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

            builder.ConfigureWebHostDefaults(webBuilder =>
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        //var settings = config.Build();

                        var appConfigEndpoint =
                            tempConfiguration.GetSection("AppConfigBootstrap").GetValue<string>("Endpoint");
                        ArgumentNullException.ThrowIfNull(appConfigEndpoint);

                        TokenCredential credential;
                        if (env == "Development")
                        {
                            var tenantId = tempConfiguration.GetSection("LocalDevelopmentCredentials")
                                .GetValue<string>("TenantId");
                            var clientId = tempConfiguration.GetSection("LocalDevelopmentCredentials")
                                .GetValue<string>("ClientId");
                            var clientSecret = tempConfiguration.GetSection("LocalDevelopmentCredentials")
                                .GetValue<string>("ClientSecret");
                            credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                        }
                        else
                        {
                            credential = new ManagedIdentityCredential();
                        }

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
                    .UseStartup<Startup>());

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
    }
}
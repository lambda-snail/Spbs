using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Spbs.Ui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var settings = config.Build();
                            
                            var appConfigEndpoint = settings.GetSection("AppConfigBootstrap").GetValue<string>("Endpoint");
                            ArgumentNullException.ThrowIfNull(appConfigEndpoint);

                            TokenCredential credential;
                            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                            if (env == "Development")
                            {
                                var tenantId = settings.GetSection("LocalDevelopmentCredentials").GetValue<string>("TenantId");
                                var clientId = settings.GetSection("LocalDevelopmentCredentials").GetValue<string>("ClientId");
                                var clientSecret = settings.GetSection("LocalDevelopmentCredentials").GetValue<string>("ClientSecret");
                                credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                            }
                            else
                            {
                                credential = new ManagedIdentityCredential();
                            }

                            //var refreshTimer = settings.GetSection("AppConfigBootstrap").GetValue<int?>("DefaultConfigRefreshHours");
                            config.AddAzureAppConfiguration(options => 
                                options.Connect(new Uri(appConfigEndpoint), credential) // or ManagedIdentityCredential?
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
    }
}
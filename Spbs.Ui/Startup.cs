using System;
using BlazorBootstrap;
using FluentValidation;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Shared.Utilities;
using Shared.Utilities.OptionsExtensions;
using Spbs.Shared.Data;
using Spbs.Ui.ComponentServices;
using Spbs.Ui.Data;
using Spbs.Ui.Features.BankIntegration;
using Spbs.Ui.Features.BankIntegration.ImportExpenses;
using Spbs.Ui.Features.BankIntegration.Models;
using Spbs.Ui.Features.BankIntegration.Models.Validation;
using Spbs.Ui.Features.BankIntegration.Services;
using Spbs.Ui.Features.Expenses;
using Spbs.Ui.Features.Expenses.Repositories;
using Spbs.Ui.Features.RecurringExpenses;
using Spbs.Ui.Middleware;

namespace Spbs.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("Spbs:AzureAd"));
            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            
            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });
            
            services.AddAzureAppConfiguration();
            
            RegisterDatabaseConnections(services);
            RegisterRepositories(services);
            RegisterUtilities(services);
            RegisterValidators(services);
            RegisterConfigurations(services);

            services.AddRazorPages();
            services.AddBlazorBootstrap();
            
            var blazorBuilder = services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            if (env == "Development")
            {
                blazorBuilder.AddCircuitOptions(options => options.DetailedErrors = true);
            }
            
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IEulaService, EulaService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INordigenLinkWriterRepository, NordigenLinkWriterRepository>();
            services.AddScoped<INordigenAccountLinkService, NordigenAccountLinkService>();
            services.AddScoped<IRedirectLinkService, RedirectLinkService>();
            
            services.RegisterNordigenIntegration(Configuration, "Spbs:NordigenOptions");
        }

        /// <summary>
        /// Validators may be used to validate some configurations on startup, so this method must be called before
        /// RegisterConfigurations.
        /// </summary>
        private void RegisterValidators(IServiceCollection services)
        {
            services.AddSingleton<IValidator<DataConfigurationOptions>, DataConfigurationOptionsValidator>();
            services.AddSingleton<IValidator<NordigenEula>, NordigenEulaFluentValidation>();
            services.AddSingleton<IValidator<TransactionsRequestParameters>, TransactionsParametersRequestFluentValidation>();
        }

        private void RegisterConfigurations(IServiceCollection services)
        {
            services.AddOptions<DataConfigurationOptions>()
                .BindConfiguration("Spbs:Data")
                .ValidateFluently()
                .ValidateOnStart();
        }

        private void RegisterUtilities(IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ImportExpensesStateManager>();
            
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IExpenseReaderRepository, ExpenseReader>();
            services.AddTransient<IExpenseWriterRepository, ExpenseWriter>();
            
            services.AddTransient<IRecurringExpenseReaderRepository, RecurringExpenseReaderRepository>();
            services.AddTransient<IRecurringExpenseWriterRepository, RecurringExpenseWriterRepository>();

            services.AddTransient<INordigenEulaWriterRepository, NordigenEulaWriterRepository>();
            services.AddTransient<INordigenEulaReaderRepository, NordigenEulaReaderRepository>();
            
            services.AddTransient<INordigenLinkWriterRepository, NordigenLinkWriterRepository>();
            services.AddTransient<INordigenLinkReaderRepository, NordigenLinkReaderRepository>();
        }

        private void RegisterDatabaseConnections(IServiceCollection services)
        {
            var mysqlConnectionString = Configuration.GetSection("Spbs:ConnectionStrings").GetValue<string>("SpbsExpenses");
            ArgumentNullException.ThrowIfNull(mysqlConnectionString); 

            var serverVersion = MySqlServerVersion.AutoDetect(mysqlConnectionString);

            services.AddDbContextFactory<ExpensesDbContext>(o => o
                .UseMySql(mysqlConnectionString, serverVersion)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                
                #if DEBUG
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine)
                #endif
            ).AddDbContextFactory<RecurringExpensesDbContext>(o => o
                .UseMySql(mysqlConnectionString, serverVersion, options =>
                {
                    options.EnableRetryOnFailure(3);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)

                #if DEBUG
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine)
                #endif
            );

            var cosmosDbConnectionString =
                Configuration.GetSection("Spbs:ConnectionStrings").GetValue<string>("CosmosDb");
            services.AddSingleton<CosmosClient>(
                new CosmosClient(
                    connectionString: cosmosDbConnectionString
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            // Use Azure App Configuration middleware for dynamic configuration refresh.
            // For options registered with IOptionsSnapshot<T>
            //app.UseAzureAppConfiguration();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging(config =>
            {
                config.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} response was: {StatusCode} in {Elapsed} ms, from userid: {UserId}";
                config.EnrichDiagnosticContext = UserInformationLogEnricher.PushSeriLogProperties;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
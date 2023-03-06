using System;
using Integrations.Nordigen;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Utilities;
using Spbs.Ui.Data;
using Spbs.Ui.Features.Expenses;
using Spbs.Ui.Features.RecurringExpenses;

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
            
            services.AddRazorPages();
            services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            services.AddAutoMapper(typeof(Startup));

            services.RegisterNordigenIntegration(Configuration, "Spbs:NordigenOptions");
        }

        private void RegisterUtilities(IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IExpenseReaderRepository, ExpenseReaderRepository>();
            services.AddTransient<IExpenseWriterRepository, ExpenseWriterRepository>();
            
            services.AddTransient<IRecurringExpenseReaderRepository, RecurringExpenseReaderRepository>();
            services.AddTransient<IRecurringExpenseWriterRepository, RecurringExpenseWriterRepository>();
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
using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spbs.Ui.Data;
using Spbs.Ui.Features.Expenses;

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
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            
            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });

            RegisterDatabaseConnections(services);
            RegisterRepositories(services);
            
            services.AddRazorPages();
            services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();
            services.AddSingleton<WeatherForecastService>();

            services.AddAutoMapper(typeof(Startup));
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IExpenseReaderRepository, ExpenseReaderRepository>();
            services.AddTransient<IExpenseWriterRepository, ExpenseWriterRepository>();
        }

        private void RegisterDatabaseConnections(IServiceCollection services)
        {
            var serverVersion = MySqlServerVersion.AutoDetect(Configuration.GetConnectionString("SpbsExpenses"));

            services.AddPooledDbContextFactory<ExpensesDbContext>(o => o
                .UseMySql(Configuration.GetConnectionString("SpbsExpenses"), serverVersion)
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
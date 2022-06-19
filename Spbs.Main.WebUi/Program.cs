using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Services;
using Spbs.Main.Core.Settings;
using Spbs.Main.InfraStructure.Identity;
using Spbs.Main.InfraStructure.Persistence;
using Spbs.Main.InfraStructure.Utilities;
using Spbs.Main.WebUi.Areas.Identity;
using Spbs.Main.WebUi.Contracts;
using Spbs.Main.WebUi.Data;
using Spbs.Main.WebUi.Services;
using Spbs.Main.WebUi.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Settings
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true).Build();
//.AddKeyVault()

builder.Services.Configure<IdentitySettings>(
    config.GetSection("IdentityDatabase"));

builder.Services.Configure<SpbsDatabaseSettings>(
    config.GetSection("SpbsMainDatabase"));

builder.Services.Configure<SpbsDatabaseSettings>(
    config.GetSection(""));

var connectionString = config.GetSection("IdentityDatabase").GetValue<string>("ConnectionString");
builder.Services.AddDbContext<UserIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserIdentityDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Mongo Db
BsonClassMapRegistrator.RegisterBsonClassMaps();
var mongoConnectionString = config.GetSection("SpbsMainDatabase").GetValue<string>("ConnectionString");
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoConnectionString));

// Core services
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();
builder.Services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Automapper
var mapperConfig = new MapperConfiguration(mapperconfig =>
{
    mapperconfig.AddProfile(new AutoMapperProfiles());
    mapperconfig.AddProfile(new ViewModelMapperProfiles());
});

builder.Services.AddSingleton(mapperConfig.CreateMapper());

// MediatR
builder.Services.AddMediatR(typeof(UpsertPurchase).Assembly);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
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

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
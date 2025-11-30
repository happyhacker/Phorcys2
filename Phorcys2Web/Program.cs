using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phorcys.Services;
using Phorcys.Data;
using Serilog;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = new Uri("https://PhorcysKeyVault.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Home/LogFiles/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Add this line to use Serilog as the logging provider

// Add services to the container
builder.Services.AddDbContext<PhorcysContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PhorcysDbConnection"),
        sqlServerOptions =>
        {
            sqlServerOptions.MigrationsAssembly("Phorcys.Web");
            sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        })
);

// Add custom services
builder.Services.AddScoped<IChecklistServices, ChecklistServices>();
builder.Services.AddScoped<ChecklistServices>();
builder.Services.AddScoped<DiveTypeServices>();
builder.Services.AddScoped<DivePlanServices>();
builder.Services.AddScoped<DiveSiteServices>();
builder.Services.AddScoped<DiveServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<LocationServices>();
builder.Services.AddScoped<GearServices>();
builder.Services.AddScoped<MyCertificationServices>();
builder.Services.AddScoped<AgencyServices>();
builder.Services.AddScoped<InstructorServices>();
builder.Services.AddScoped<ContactServices>();

// Configure Identity with email confirmation
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Require confirmed account
    options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation"; // Map to the named token provider
})
.AddEntityFrameworkStores<PhorcysContext>()
.AddDefaultTokenProviders();

// Explicitly map the "emailconfirmation" token provider
builder.Services.AddTransient<IUserTwoFactorTokenProvider<IdentityUser>, DataProtectorTokenProvider<IdentityUser>>();

// Configure token lifespan for email confirmation
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24); // Token is valid for 24 hours
});

// Register "emailconfirmation" as a token provider
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Tokens.ProviderMap.Add("emailconfirmation", new TokenProviderDescriptor(
        typeof(DataProtectorTokenProvider<IdentityUser>)));
});

// Configure email sender service
builder.Services.AddTransient<IEmailSender, EmailSender>(); // Add custom email sender

// Add MVC and Kendo UI
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddKendo();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

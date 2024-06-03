using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phorcys.Services;
using Phorcys.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*IServiceCollection serviceCollection = builder.Services.AddDbContext<PhorcysContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("PhorcysDbConnection"),
		x => x.MigrationsAssembly("Phorcys.Data")
	)
);*/
builder.Services.AddDbContext<PhorcysContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("PhorcysDbConnection"),
		x => x.MigrationsAssembly("Phorcys.Web")
	)
);

builder.Services.AddScoped<DivePlanServices>();
builder.Services.AddScoped<DiveSiteServices>();
builder.Services.AddScoped<DiveServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<LocationServices>();
builder.Services.AddScoped<GearServices>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<PhorcysContext>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddKendo();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();
//app.MapControllers();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

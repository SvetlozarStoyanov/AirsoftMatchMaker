using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AirsoftMatchmakerDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.Lockout.AllowedForNewUsers = false;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AirsoftMatchmakerDbContext>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddApplicationServices();


builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Users/Login";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
            name: "Administrator",
            areaName: "Administrator",
            pattern: "Administrator/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
            name: "Matchmaker",
            areaName: "Matchmaker",
            pattern: "Matchmaker/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
            name: "Vendor",
            areaName: "Vendor",
            pattern: "Vendor/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
            name: "Player",
            areaName: "Player",
            pattern: "Player/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

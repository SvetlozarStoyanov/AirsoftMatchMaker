using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
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
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IWeaponService, WeaponService>();
builder.Services.AddScoped<IClothingService, ClothingService>();
builder.Services.AddScoped<IAmmoBoxService, AmmoBoxService>();
builder.Services.AddScoped<IMapService, MapService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameModeService, GameModeService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IMatchmakerService, MatchmakerService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRequestService, RoleRequestService>();


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

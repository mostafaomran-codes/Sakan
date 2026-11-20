using Sakan.Areas.Accounts;
using Sakan.Areas.Bookings;
using Sakan.Areas.Finance;
using Sakan.Areas.Listings;
using Sakan.Areas.Trust;
using Sakan.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Per-developer overrides (git-ignored), e.g. a different connection string.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllersWithViews();

// Lead only: one line per module. Modules register their own services in {Module}Module.cs.
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddAccountsModule()
    .AddListingsModule()
    .AddBookingsModule()
    .AddFinanceModule()
    .AddTrustModule();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

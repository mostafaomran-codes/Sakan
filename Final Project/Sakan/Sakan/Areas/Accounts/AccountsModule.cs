using Microsoft.AspNetCore.Identity;
using Sakan.Areas.Accounts.Entities;
using Sakan.Areas.Accounts.Seeders;
using Sakan.Data;
using Sakan.Shared.Contracts;

namespace Sakan.Areas.Accounts;

// Owner: M1.
public static class AccountsModule
{
    public static IServiceCollection AddAccountsModule(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Accounts/Auth/Login";
            options.AccessDeniedPath = "/Accounts/Auth/AccessDenied";
        });

        services.AddScoped<IDataSeeder, RoleSeeder>();

        return services;
    }
}

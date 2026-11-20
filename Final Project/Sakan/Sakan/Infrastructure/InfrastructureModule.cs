using Microsoft.EntityFrameworkCore;
using Sakan.Data;
using Sakan.Shared.Constants;
using Sakan.Shared.Contracts;
using Sakan.Shared.Options;

namespace Sakan.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // NOTE: do NOT enable EnableRetryOnFailure(): it breaks manual transactions
        // (booking creation, financial operations) unless wrapped in an execution strategy.
        services.AddDbContext<AppDbContext>(o => o
            .UseSqlServer(connectionString)
            .ConfigureWarnings(w => w.Ignore(
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

        services.Configure<PlatformOptions>(config.GetSection(PlatformOptions.SectionName));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IClock, UtcClock>();
        services.AddSingleton<IFileStorage, LocalFileStorage>();

        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.AdminOnly, p => p.RequireRole(Roles.Admin))
            .AddPolicy(Policies.HostOnly, p => p.RequireRole(Roles.Host));

        return services;
    }
}

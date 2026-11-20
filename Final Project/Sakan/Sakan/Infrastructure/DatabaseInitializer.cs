using Microsoft.EntityFrameworkCore;
using Sakan.Data;
using Sakan.Shared.Contracts;

namespace Sakan.Infrastructure;

public static class DatabaseInitializer
{
    /// <summary>Development: applies pending migrations. Always: runs every registered IDataSeeder.</summary>
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var sp = scope.ServiceProvider;

        if (app.Environment.IsDevelopment())
        {
            var db = sp.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }

        foreach (var seeder in sp.GetServices<IDataSeeder>())
            await seeder.SeedAsync();
    }
}

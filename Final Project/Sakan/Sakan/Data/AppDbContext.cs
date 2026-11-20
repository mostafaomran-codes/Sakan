using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sakan.Areas.Accounts.Entities;

namespace Sakan.Data;

// Lead only. Each module adds its DbSets in its own AppDbContext.{Module}.cs file.
// Entity mapping goes in IEntityTypeConfiguration classes (picked up automatically).
// Enums are stored as strings automatically: do NOT call HasConversion on enums.
// In every configuration ALWAYS call OnDelete(...) explicitly: Restrict by default,
// Cascade only for pure children (images, amenity links, shared spaces, address, policy rules).
public partial class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // All enum properties -> string (max 30)
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties()))
        {
            var type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
            if (!type.IsEnum) continue;

            property.SetProviderClrType(typeof(string));
            property.SetMaxLength(30);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // All money fields. Override per property if needed (e.g. latitude/longitude).
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}

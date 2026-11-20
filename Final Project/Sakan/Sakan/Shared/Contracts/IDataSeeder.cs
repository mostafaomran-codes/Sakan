namespace Sakan.Shared.Contracts;

/// <summary>
/// Idempotent startup seeding. Register with services.AddScoped&lt;IDataSeeder, YourSeeder&gt;()
/// inside your module extension. Seeders run in registration order (Accounts first).
/// </summary>
public interface IDataSeeder
{
    Task SeedAsync(CancellationToken ct = default);
}

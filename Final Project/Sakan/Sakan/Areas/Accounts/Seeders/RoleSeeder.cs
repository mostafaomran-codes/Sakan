using Microsoft.AspNetCore.Identity;
using Sakan.Shared.Constants;
using Sakan.Shared.Contracts;

namespace Sakan.Areas.Accounts.Seeders;

public class RoleSeeder(RoleManager<IdentityRole> roleManager) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

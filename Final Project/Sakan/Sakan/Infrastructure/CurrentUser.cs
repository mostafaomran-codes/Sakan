using System.Security.Claims;
using Sakan.Shared.Contracts;

namespace Sakan.Infrastructure;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string RequiredUserId => UserId ?? throw new InvalidOperationException("No authenticated user.");
    public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;
}

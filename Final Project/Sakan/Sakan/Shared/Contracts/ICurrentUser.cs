namespace Sakan.Shared.Contracts;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UserId { get; }

    /// <summary>Throws if nobody is signed in. Use inside [Authorize] actions.</summary>
    string RequiredUserId { get; }

    bool IsInRole(string role);
}

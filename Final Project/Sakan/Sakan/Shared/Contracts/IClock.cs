namespace Sakan.Shared.Contracts;

/// <summary>Always use this instead of DateTime.UtcNow so time-based rules can be tested.</summary>
public interface IClock
{
    DateTime UtcNow { get; }
}

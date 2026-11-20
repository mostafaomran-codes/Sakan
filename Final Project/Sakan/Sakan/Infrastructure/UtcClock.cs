using Sakan.Shared.Contracts;

namespace Sakan.Infrastructure;

public class UtcClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}

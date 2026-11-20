namespace Sakan.Shared.Options;

/// <summary>Business settings, bound from the "Platform" section of appsettings.json.</summary>
public class PlatformOptions
{
    public const string SectionName = "Platform";

    /// <summary>Fraction (0.10 = 10%) taken from the amount released to the host.</summary>
    public decimal CommissionRate { get; set; } = 0.10m;

    public int PendingApprovalHours { get; set; } = 24;
    public int IssueWindowHours { get; set; } = 24;
    public int DamageWindowHours { get; set; } = 48;

    /// <summary>Pick the tier with the largest MinHoursBeforeCheckIn that is &lt;= hours remaining.</summary>
    public List<HostPenaltyTier> HostPenaltyTiers { get; set; } = [];
}

public class HostPenaltyTier
{
    public int MinHoursBeforeCheckIn { get; set; }

    /// <summary>Fraction of the cancelled booking's RentalAmount.</summary>
    public decimal PenaltyRate { get; set; }
}

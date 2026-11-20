namespace Sakan.Shared.Enums;

public enum PaymentAuthorizationStatus { Authorized, PartiallyCaptured, Captured, Released }
public enum DepositStatus { Authorized, PartiallyCaptured, Captured, Released }
public enum TransactionType
{
    Authorization, AuthorizationRelease, Capture, Refund,
    SecurityCapture, SecurityRelease, Payout, PlatformFee, PenaltyDeduction
}
public enum TransactionStatus { Pending, Completed, Failed }
public enum PayoutType { RentalRelease, DamageCompensation }
public enum PayoutStatus { Pending, OnHold, Paid, Cancelled, Failed }
public enum PenaltyStatus { Open, Settled, Waived }

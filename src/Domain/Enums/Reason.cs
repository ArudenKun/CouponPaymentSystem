using Ardalis.SmartEnum;

namespace CouponPaymentSystem.Domain.Enums;

public sealed class Reason : SmartEnum<Reason>
{
    public static readonly Reason None = new(nameof(None), 0);
    public static readonly Reason InvalidAccountNumber = new("Invalid Account Number", 1);
    public static readonly Reason AccountNameMismatch = new("Account Name Mismatch", 2);

    private Reason(string name, int value)
        : base(name, value) { }
}

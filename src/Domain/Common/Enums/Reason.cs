using Ardalis.SmartEnum;

namespace Domain.Common.Enums;

public class Reason : SmartEnum<Reason, string>
{
    public static readonly Reason InvalidAccountNumber = new(
        nameof(InvalidAccountNumber),
        "Invalid Account Number"
    );

    public static readonly Reason AccountNameMismatch = new(
        nameof(AccountNameMismatch),
        "Account Name Mismatch"
    );

    protected Reason(string name, string value)
        : base(name, value) { }
}

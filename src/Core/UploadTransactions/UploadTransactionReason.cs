using Ardalis.SmartEnum;

namespace CouponPaymentSystem.Core.UploadTransactions;

public sealed class UploadTransactionReason : SmartEnum<UploadTransactionReason>
{
    public static readonly UploadTransactionReason None = new(nameof(None), 0);
    public static readonly UploadTransactionReason InvalidAccountNumber = new(
        "Invalid Account Number",
        1
    );
    public static readonly UploadTransactionReason AccountNameMismatch = new(
        "Account Name Mismatch",
        2
    );

    private UploadTransactionReason(string name, int value)
        : base(name, value) { }
}

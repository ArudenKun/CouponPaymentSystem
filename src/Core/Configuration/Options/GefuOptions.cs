namespace CouponPaymentSystem.Core.Configuration.Options;

public sealed class GefuOptions
{
    public IReadOnlyDictionary<string, string> TransactionCodes { get; init; } =
        new Dictionary<string, string>().AsReadOnly();
}

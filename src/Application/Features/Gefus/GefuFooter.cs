namespace CouponPaymentSystem.Application.Features.Gefus;

public sealed class GefuFooter
{
    public int RecordType { get; init; }
    public int NumberOfDebits { get; init; }
    public decimal TotalDebitAmountInLocalCurrency { get; init; }
    public int NumberOfCredits { get; init; }
    public decimal TotalCreditAmountInLocalCurrency { get; init; }
}

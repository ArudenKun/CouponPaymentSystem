namespace CouponPaymentSystem.Application.Common.Configurations;

public interface IGefuSettings
{
    string GefuFileNameFormat { get; init; }
    string TransactionType { get; init; }
    string BranchCode { get; init; }
    string TransactionDescription { get; init; }
    string OptionsFlag { get; init; }
    string DebitCreditFlag { get; init; }
    IReadOnlyDictionary<string, string> TransactionCode { get; init; }
}

using CouponPaymentSystem.Application.Common.Configurations;

namespace CouponPaymentSystem.Infrastructure.Configurations;

internal sealed class GefuSettings : IGefuSettings
{
    public string GefuFileNameFormat { get; init; } = "GEFUUP_CPS_{0}_{1}.txt";
    public string TransactionType { get; init; } = "01";
    public string BranchCode { get; init; } = "0063";
    public string TransactionDescription { get; init; } = "COUPON PAYMENT";
    public string OptionsFlag { get; init; } = "10";
    public string DebitCreditFlag { get; init; } = "C";

    public IReadOnlyDictionary<string, string> TransactionCode { get; init; } =
        new Dictionary<string, string>
        {
            { "00600", "Ordinary Credit" },
            { "00655", "CM INT_INC_SOV BOND" },
            { "00656", "CM INT_INC CORP BOND" },
            { "00657", "CM INT_INC_LTNCD" },
            { "00658", "CM PYMT_SOV BOND" },
            { "00659", "CM PYMT CORP BOND" },
            { "00660", "CM PYMT LTNCD" },
        }.AsReadOnly();
}

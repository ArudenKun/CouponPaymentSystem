namespace CouponPaymentSystem.Application.Features.Gefus;

public sealed class GefuEntry
{
    public long RecordType { get; init; }
    public string TransactionType { get; init; } = string.Empty;
    public string AccountNumber { get; init; } = string.Empty;
    public string BranchCode { get; init; } = string.Empty;
    public string TransactionCode { get; init; } = string.Empty;
    public DateTime TransactionDate { get; init; }
    public char DebitCreditFlag { get; init; } = 'C';
    public DateTime ValueDate { get; init; }
    public string TransactionCurrency { get; init; } = string.Empty;
    public decimal AmountInLocalCurrency { get; init; }
    public decimal AmountInTransactionCurrency { get; init; }
    public string ConversionRate { get; init; } = string.Empty;
    public string ReferenceNumber { get; init; } = string.Empty;
    public string DocumentNumber { get; init; } = string.Empty;
    public string TransactionDescription { get; init; } = string.Empty;
    public string BeneficiaryIc { get; init; } = string.Empty;
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryAddress1 { get; init; } = string.Empty;
    public string BeneficiaryAddress2 { get; init; } = string.Empty;
    public string BeneficiaryAddress3 { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string OptionsFlag { get; init; } = string.Empty;
    public string IssueCode { get; init; } = string.Empty;
    public string PayableBranch { get; init; } = string.Empty;
    public char FutureDateFlag { get; init; } = 'N';
    public string MisCode { get; init; } = string.Empty;
    public char ProcessingFlag { get; init; } = ' ';
    public string ReasonFlag { get; init; } = string.Empty;
    public string Udt1 { get; init; } = string.Empty;
    public string Udt2 { get; init; } = string.Empty;
    public string Udt3 { get; init; } = string.Empty;
    public string Udt4 { get; init; } = string.Empty;
    public string Udt5 { get; init; } = string.Empty;
    public string Udt6 { get; init; } = string.Empty;
    public string Udt7 { get; init; } = string.Empty;
    public string Udt8 { get; init; } = string.Empty;
    public string Udt9 { get; init; } = string.Empty;
    public string Udt10 { get; init; } = string.Empty;
    public string Udt11 { get; init; } = string.Empty;
    public string Udt12 { get; init; } = string.Empty;
    public string Udt13 { get; init; } = string.Empty;
};

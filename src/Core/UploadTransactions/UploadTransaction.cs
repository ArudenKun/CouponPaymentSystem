using Abp.Domain.Entities;
using CouponPaymentSystem.Core.Uploads;

namespace CouponPaymentSystem.Core.UploadTransactions;

public class UploadTransaction : Entity
{
    public virtual required string AccountNumber { get; set; }
    public virtual required string NewAccountNumber { get; set; }
    public virtual required string AccountName { get; set; }
    public virtual required string CasaAccountName { get; set; }
    public virtual string? AccountType { get; set; }
    public virtual required decimal TransactionAmount { get; set; }
    public virtual required decimal NetAmount { get; set; }
    public virtual bool IsMatch { get; set; }
    public virtual bool IsRejected { get; set; }
    public virtual UploadTransactionReason Reason { get; set; } = UploadTransactionReason.None;
    public virtual Upload Upload { get; set; } = null!;
}

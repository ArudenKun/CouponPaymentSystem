using Abp.Domain.Entities;

namespace CouponPaymentSystem.Core.GrsAlliedAccounts;

public class GrsAlliedAccount : Entity<string>
{
    // public virtual required string AccountNumber { get; set; }
    public virtual string AccountName1 { get; set; } = string.Empty;
    public virtual string? AccountName2 { get; set; }
    public virtual string? AccountName3 { get; set; }
    public virtual string? NewAccountNumber { get; set; }
    public virtual string? CurrencyCode { get; set; }
    public virtual string? BranchCode { get; set; }
}

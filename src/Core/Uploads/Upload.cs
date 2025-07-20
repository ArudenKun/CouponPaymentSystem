using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CouponPaymentSystem.Core.Authorization;
using CouponPaymentSystem.Core.UploadTransactions;

namespace CouponPaymentSystem.Core.Uploads;

public class Upload : Entity, IMustHaveTenant, IHasCreationTime
{
    public virtual required DomainId DomainId { get; set; }
    public virtual string JobId { get; set; } = "None";
    public virtual int TenantId { get; set; }
    public virtual required string FileName { get; set; }
    public virtual required UploadCurrency Currency { get; set; }
    public virtual required UploadStatus Status { get; set; }

    public virtual DateTime CreationTime { get; set; }
    public virtual List<UploadTransaction> Transactions { get; init; } = [];
}

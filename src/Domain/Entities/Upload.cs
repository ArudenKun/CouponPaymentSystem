using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CouponPaymentSystem.Domain.Enums;

namespace CouponPaymentSystem.Domain.Entities;

public class Upload : Entity, IMustHaveTenant, IHasCreationTime
{
    public virtual required string DomainId { get; set; }
    public virtual string JobId { get; set; } = "None";
    public virtual int TenantId { get; set; }
    public virtual required string FileName { get; set; }
    public virtual required Currency Currency { get; set; }
    public virtual required Status Status { get; set; }

    public virtual DateTime CreationTime { get; set; }
    public virtual List<UploadTransaction> Transactions { get; init; } = [];
}

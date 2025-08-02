using Abp.NHibernate.EntityMappings;
using CouponPaymentSystem.Domain.Entities;

namespace CouponPaymentSystem.Infrastructure.Persistence.Configurations;

internal class UploadMap : EntityMap<Upload>
{
    public UploadMap()
        : base(nameof(Upload))
    {
        Id(x => x.Id).GeneratedBy.Identity();
        Map(x => x.DomainId).Not.Nullable().Length(16);
        Map(x => x.TenantId);
        Map(x => x.JobId).Not.Nullable();
        Map(x => x.FileName).Not.Nullable().Unique().Length(60);
        Map(x => x.Currency).Not.Nullable().Length(16);
        Map(x => x.Status).Not.Nullable().Length(60);
        HasMany(x => x.Transactions).Cascade.All();

        this.MapCreationTime();
    }
}

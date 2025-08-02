using Abp.NHibernate.EntityMappings;
using CouponPaymentSystem.Domain.Entities;

namespace CouponPaymentSystem.Infrastructure.Persistence.Configurations;

internal class SettingMap : EntityMap<Setting, long>
{
    public SettingMap()
        : base(nameof(Setting))
    {
        Map(x => x.TenantId);
        Map(x => x.UserId);
        Map(x => x.Name).Length(Setting.MaxNameLength).Not.Nullable();
        Map(x => x.Value);

        this.MapAudited();
    }
}

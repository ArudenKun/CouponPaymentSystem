using Abp.NHibernate.EntityMappings;

namespace CouponPaymentSystem.Core.Configuration;

public class SettingMap : EntityMap<Setting, long>
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

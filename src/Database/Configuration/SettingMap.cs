using Abp.NHibernate.EntityMappings;
using CouponPaymentSystem.Core.Configuration;

namespace CouponPaymentSystem.Database.Configuration;

public class SettingMap : EntityMap<Setting, long>
{
    public SettingMap()
        : base(nameof(Setting))
    {
        Map(x => x.TenantId);
        Map(x => x.UserId);
        Map(x => x.Name).Length(Setting.MaxNameLength).Not.Nullable();
        Map(x => x.Value).Length(DatabaseConstants.NvarcharMax);

        this.MapAudited();
    }
}

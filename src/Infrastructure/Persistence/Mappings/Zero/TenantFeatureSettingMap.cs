using Abp.MultiTenancy;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class TenantFeatureSettingMap : SubclassMap<TenantFeatureSetting>
{
    public TenantFeatureSettingMap()
    {
        DiscriminatorValue("TenantFeatureSetting");
    }
}

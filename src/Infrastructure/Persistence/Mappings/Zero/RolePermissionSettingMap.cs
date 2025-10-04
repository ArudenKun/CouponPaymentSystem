using Abp.Authorization.Roles;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class RolePermissionSettingMap : SubclassMap<RolePermissionSetting>
{
    public RolePermissionSettingMap()
    {
        DiscriminatorValue("RolePermissionSetting");

        Map(x => x.RoleId).Not.Nullable();
    }
}

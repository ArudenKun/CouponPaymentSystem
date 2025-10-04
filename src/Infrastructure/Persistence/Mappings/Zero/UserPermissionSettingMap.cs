using Abp.Authorization.Users;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class UserPermissionSettingMap : SubclassMap<UserPermissionSetting>
{
    public UserPermissionSettingMap()
    {
        DiscriminatorValue("UserPermissionSetting");

        Map(x => x.UserId).Not.Nullable();
    }
}

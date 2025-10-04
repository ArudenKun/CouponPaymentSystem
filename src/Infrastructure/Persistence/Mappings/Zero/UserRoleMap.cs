using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class UserRoleMap : EntityMap<UserRole, long>
{
    public UserRoleMap()
        : base("AbpUserRoles")
    {
        Map(x => x.TenantId);
        Map(x => x.UserId).Not.Nullable();
        Map(x => x.RoleId).Not.Nullable();

        this.MapCreationAudited();
    }
}

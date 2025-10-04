using Domain.Roles;
using Domain.Users;
using Infrastructure.Persistence.Mappings.Zero;

namespace Infrastructure.Persistence.Mappings;

internal class RoleMap : AbpRoleMap<Role, User>;

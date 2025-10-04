using Abp.Authorization;
using Domain.Roles;
using Domain.Users;

namespace Domain.Authorization;

internal class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager) { }
}

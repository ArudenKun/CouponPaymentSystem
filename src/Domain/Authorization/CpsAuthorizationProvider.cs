using Abp.Authorization;

namespace Domain.Authorization;

internal class CpsAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        context.CreatePermission(Permissions.Checker);
        context.CreatePermission(Permissions.Maker);
    }
}

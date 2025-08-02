using System.Security.Claims;
using Abp;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Runtime.Session;

namespace CouponPaymentSystem.Application.Authorization;

public class PermissionChecker : IPermissionChecker
{
    private readonly IPrincipalAccessor _principalAccessor;
    private readonly IPermissionManager _permissionManager;

    public PermissionChecker(
        IPrincipalAccessor principalAccessor,
        IPermissionManager permissionManager
    )
    {
        _principalAccessor = principalAccessor;
        _permissionManager = permissionManager;
    }

    public Task<bool> IsGrantedAsync(string permissionName) =>
        Task.FromResult(IsGranted(permissionName));

    public bool IsGranted(string permissionName)
    {
        var permission = _permissionManager.GetPermissionOrNull(permissionName);
        if (
            permission?.Properties["Roles"] is not List<string> grantedRoles
            || grantedRoles.Count is 0
        )
            return false;

        var userRoles = _principalAccessor
            .Principal.Claims.Where(x => x.Type is ClaimTypes.Role)
            .Select(x => x.Value)
            .Where(x => !x.IsNullOrWhiteSpace())
            .ToArray();

        return userRoles.Length > 0 && grantedRoles.Intersect(userRoles).Any();
    }

    public Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName) =>
        IsGrantedAsync(permissionName);

    public bool IsGranted(UserIdentifier user, string permissionName) => IsGranted(permissionName);
}

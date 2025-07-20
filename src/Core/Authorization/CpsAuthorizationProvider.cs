using Abp.Authorization;
using Abp.Localization;
using CouponPaymentSystem.Core.Configuration.Options;

namespace CouponPaymentSystem.Core.Authorization;

public class CpsAuthorizationProvider : AuthorizationProvider
{
    private readonly CpsOptions _cpsOptions;

    public CpsAuthorizationProvider(CpsOptions cpsOptions)
    {
        _cpsOptions = cpsOptions;
    }

    public override void SetPermissions(IPermissionDefinitionContext context) =>
        context.CreatePermission(
            PermissionNames.Checker,
            L("Checker"),
            properties: new Dictionary<string, object>
            {
                {
                    "Roles",
                    new List<string> { _cpsOptions.Checker }
                },
            }
        );

    private static ILocalizableString L(string name) =>
        new LocalizableString(name, CpsConstants.LocalizationSourceName);
}

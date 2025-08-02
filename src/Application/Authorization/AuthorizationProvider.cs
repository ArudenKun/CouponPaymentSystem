using Abp.Authorization;
using Abp.Localization;
using CouponPaymentSystem.Application.Common.Configurations;
using CouponPaymentSystem.Domain;

namespace CouponPaymentSystem.Application.Authorization;

internal sealed class AuthorizationProvider : Abp.Authorization.AuthorizationProvider
{
    private readonly IApplicationSettings _settings;

    public AuthorizationProvider(IApplicationSettings settings)
    {
        _settings = settings;
    }

    public override void SetPermissions(IPermissionDefinitionContext context) =>
        context.CreatePermission(
            PermissionNames.Checker,
            L("Checker"),
            properties: new Dictionary<string, object>
            {
                {
                    "Roles",
                    new List<string> { _settings.Checker }
                },
            }
        );

    private static ILocalizableString L(string name) =>
        new LocalizableString(name, CpsConstants.LocalizationSourceName);
}

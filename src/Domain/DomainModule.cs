using System.Reflection;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using Domain.Authorization;
using Domain.Roles;
using Domain.Tenants;
using Domain.Users;

namespace Domain;

[DependsOn(typeof(AbpZeroCoreModule))]
public sealed class DomainModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        Configuration.MultiTenancy.IsEnabled = CpsConstants.MultiTenancyEnabled;

        Configuration.Authorization.Providers.Add<CpsAuthorizationProvider>();

        // Configuration.Settings.Providers.Add<AppSettingProvider>();

        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase =
            CpsConstants.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
    }
}

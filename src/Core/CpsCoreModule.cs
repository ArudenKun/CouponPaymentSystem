using Abp;
using Abp.Modules;
using Abp.Runtime.Security;
using Abp.Timing;
using Aoxe.Snowflake;
using Castle.MicroKernel.Registration;
using CouponPaymentSystem.Core.Authorization;
using CouponPaymentSystem.Core.Common;
using CouponPaymentSystem.Core.Common.Extensions;
using CouponPaymentSystem.Core.Configuration.Options;
using CouponPaymentSystem.Core.Timing;

namespace CouponPaymentSystem.Core;

[DependsOn(typeof(AbpKernelModule))]
public class CpsCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        IocManager.Register<IPathManager, PathManager>();
        IocManager.Register(
            Component
                .For<CpsOptions>()
                .DependsOn(
                    Dependency.OnValue("path", IocManager.Resolve<IPathManager>().ConfigPath)
                )
                .LifestyleSingleton(),
            Component
                .For<SnowflakeIdGenerator>()
                .UsingFactoryMethod(() => new SnowflakeIdGenerator(1, 1))
        );
        Configuration.DefaultNameOrConnectionString = IocManager
            .Resolve<CpsOptions>()
            .Cps.ConnectionString;
        Configuration.MultiTenancy.IsEnabled = CpsConstants.MultiTenancyEnabled;
        Configuration.Authorization.Providers.Add<CpsAuthorizationProvider>();
        // Configuration.Settings.Providers.Add<AppSettingProvider>();
        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase =
            CpsConstants.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = CpsConstants.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(CpsCoreModule).Assembly;
        IocManager.RegisterAssemblyByConvention(thisAssembly);
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
    }
}

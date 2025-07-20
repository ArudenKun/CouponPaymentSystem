using Abp;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.NHibernate;
using Abp.Runtime.Security;
using Abp.Timing;
using Castle.MicroKernel.Registration;
using CouponPaymentSystem.Core.Authorization;
using CouponPaymentSystem.Core.Common;
using CouponPaymentSystem.Core.Common.Extensions;
using CouponPaymentSystem.Core.Configuration.Options;
using CouponPaymentSystem.Core.Timing;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace CouponPaymentSystem.Core;

[DependsOn(typeof(AbpKernelModule), typeof(AbpNHibernateModule))]
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
                .LifestyleSingleton()
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

        Configuration
            .Modules.AbpNHibernate()
            .FluentConfiguration.Database(
                MsSqlConfiguration.MsSql2012.ConnectionString(
                    Configuration.DefaultNameOrConnectionString
                )
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CpsCoreModule>())
            .ExposeConfiguration(c =>
            {
                using var writer = new StringWriter();
                new SchemaExport(c).Create(writer, true);
                writer.Flush();
                File.WriteAllText(
                    IocManager.Resolve<IPathManager>().AppDataDir.Combine("script.sql"),
                    writer.ToString()
                );
            });
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

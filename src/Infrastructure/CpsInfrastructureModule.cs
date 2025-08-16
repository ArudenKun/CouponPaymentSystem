using System.Data.SqlClient;
using System.Reflection;
using Abp.BlobStoring.Database.NHibernate;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.NHibernate;
using AsyncKeyedLock;
using Castle.MicroKernel.Registration;
using CouponPaymentSystem.Application;
using CouponPaymentSystem.Application.Common;
using CouponPaymentSystem.Application.Common.Configurations;
using CouponPaymentSystem.Application.Common.Extensions;
using CouponPaymentSystem.Infrastructure.Configurations;
using CouponPaymentSystem.Infrastructure.Helpers;
using CouponPaymentSystem.Infrastructure.Services;
using FluentNHibernate.Cfg.Db;
using Hangfire;
using Hangfire.SqlServer;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using ZiggyCreatures.Caching.Fusion;

namespace CouponPaymentSystem.Infrastructure;

[DependsOn(
    typeof(CpsApplicationModule),
    typeof(AbpNHibernateModule),
    typeof(AbpBlobStoringDatabaseNHibernateModule),
    typeof(AbpHangfireModule)
)]
public class CpsInfrastructureModule : AbpModule
{
    private static readonly Assembly ThisAssembly = typeof(CpsInfrastructureModule).Assembly;

    public override void PreInitialize()
    {
        IocManager.Register<IApplicationSettings, ApplicationSettings>();
        IocManager.Register<IPathManager, PathManager>();
        Configuration.DefaultNameOrConnectionString = IocManager
            .Resolve<IApplicationSettings>()
            .Cps.ConnectionString;

        Configuration.BackgroundJobs.UseHangfire(c =>
        {
            var sqlServerOptions = new SqlServerStorageOptions
            {
                SqlClientFactory = SqlClientFactory.Instance,
                PrepareSchemaIfNecessary = DebugHelper.IsDebug,
            };
            c.GlobalConfiguration.UseSqlServerStorage(
                Configuration.DefaultNameOrConnectionString,
                sqlServerOptions
            );
        });

        Configuration
            .Modules.AbpNHibernate()
            .FluentConfiguration.Database(
                MsSqlConfiguration
                    .MsSql2012.Driver<MicrosoftDataSqlClientDriver>()
                    .ConnectionString(Configuration.DefaultNameOrConnectionString)
            )
            .Mappings(m => m.FluentMappings.AddFromAssembly(ThisAssembly))
            .ExposeConfiguration(c =>
            {
                using var writer = new StreamWriter(
                    IocManager.Resolve<IPathManager>().AppDataDir.Combine("script.sql")
                );
                new SchemaExport(c).Create(writer, true);
            });
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(ThisAssembly);
        IocManager.IocContainer.Register(
            Component
                .For<AsyncKeyedLocker<string>>()
                .ImplementedBy<AsyncKeyedLocker<string>>()
                .LifestyleSingleton(),
            Component
                .For<IDbConnectionFactory<SqlConnection>>()
                .ImplementedBy<DbConnectionFactory<SqlConnection>>()
                .DependsOn(
                    (kernel, _) =>
                        Dependency.OnValue(
                            "connectionString",
                            kernel.Resolve<IApplicationSettings>().Aso.ConnectionString
                        )
                )
                .LifestyleSingleton()
        );
        IocManager.RegisterWithServiceCollection(services =>
            services
                .AddFusionCache()
                .WithDefaultEntryOptions(options => options.SetDuration(30.Minutes()))
        );
    }
}

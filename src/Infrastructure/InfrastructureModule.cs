using System.Data.SqlClient;
using System.Reflection;
using System.Web.Hosting;
using Abp;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.EntityHistory;
using Abp.Modules;
using Abp.NHibernate;
using Application;
using Castle.Windsor.Microsoft.DependencyInjection;
using Cloaked;
using Cloaked.Sqids;
using Domain.Common;
using Domain.Configuration;
using FluentNHibernate.Cfg.Db;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Persistence;
using Infrastructure.Persistence.EventListeners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NHibernate.Event;

namespace Infrastructure;

[DependsOn(typeof(ApplicationModule), typeof(AbpHangfireModule), typeof(AbpNHibernateModule))]
public sealed class InfrastructureModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.ReplaceService<IEntityHistoryStore, NhEntityHistoryStore>(
            DependencyLifeStyle.Transient
        );
        IocManager.Register<IEntityHistoryHelper, NhEntityHistoryHelper>(
            DependencyLifeStyle.Transient
        );
        IocManager.Register<IPreInsertEventListener, InsertEventListener>(
            DependencyLifeStyle.Transient
        );
        IocManager.Register<IPreUpdateEventListener, UpdateEventListener>(
            DependencyLifeStyle.Transient
        );
        IocManager.Register<IPreDeleteEventListener, DeleteEventListener>(
            DependencyLifeStyle.Transient
        );
        IocManager.Register<IFlushEventListener, FlushEventListener>(DependencyLifeStyle.Transient);

        var basePath = HostingEnvironment.MapPath("~/") ?? AppDomain.CurrentDomain.BaseDirectory;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{EnvironmentHelper.EnvironmentName}.json", true, true)
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        services.AddSingleton<IConfiguration>(sp => sp.GetRequiredService<IConfigurationRoot>());
        services.ConfigureOptions<ValidateCpsOptions>();
        services.AddConfiguration();
        IocManager.IocContainer.AddServices(services);

        Configuration.DefaultNameOrConnectionString = IocManager
            .Resolve<IOptions<CpsOptions>>()
            .Value.Cps.ConnectionString;

        Configuration.BackgroundJobs.UseHangfire(c =>
        {
            var options = new SqlServerStorageOptions
            {
                SqlClientFactory = SqlClientFactory.Instance,
                PrepareSchemaIfNecessary = false,
            };
            c.GlobalConfiguration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.DefaultNameOrConnectionString, options);
        });
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        var services = new ServiceCollection();
        services
            .AddCloak()
            .WithSqids(options =>
            {
                options.MinLength = 8;
            });
        IocManager.IocContainer.AddServices(services);

        Configuration
            .Modules.AbpNHibernate()
            .FluentConfiguration.Database(
                MsSqlConfiguration.MsSql2012.ConnectionString(
                    Configuration.DefaultNameOrConnectionString
                )
            )
            .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<InfrastructureModule>()
                    .Conventions.AddFromAssemblyOf<InfrastructureModule>()
            )
            .ExposeConfiguration(c =>
            {
                c.AppendListeners(
                    ListenerType.PreInsert,
                    [IocManager.Resolve<IPreInsertEventListener>()]
                );
                c.AppendListeners(
                    ListenerType.PreUpdate,
                    [IocManager.Resolve<IPreUpdateEventListener>()]
                );
                c.AppendListeners(
                    ListenerType.PreDelete,
                    [IocManager.Resolve<IPreDeleteEventListener>()]
                );
                c.AppendListeners(ListenerType.Flush, [IocManager.Resolve<IFlushEventListener>()]);
            });
    }
}

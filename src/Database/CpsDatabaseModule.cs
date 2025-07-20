using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.NHibernate;
using CouponPaymentSystem.Core;
using CouponPaymentSystem.Core.Common;
using CouponPaymentSystem.Core.Common.Extensions;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace CouponPaymentSystem.Database;

[DependsOn(typeof(AbpNHibernateModule), typeof(CpsCoreModule))]
public class CpsDatabaseModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration
            .Modules.AbpNHibernate()
            .FluentConfiguration.Database(
                MsSqlConfiguration.MsSql2012.ConnectionString(
                    Configuration.DefaultNameOrConnectionString
                )
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CpsDatabaseModule>())
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
        var thisAssembly = typeof(CpsDatabaseModule).Assembly;
        IocManager.RegisterAssemblyByConvention(thisAssembly);
    }
}

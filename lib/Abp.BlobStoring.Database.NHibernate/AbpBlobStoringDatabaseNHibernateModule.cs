using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.NHibernate;

namespace Abp.BlobStoring.Database.NHibernate;

[DependsOn(typeof(AbpBlobStoringDatabaseModule), typeof(AbpNHibernateModule))]
public class AbpBlobStoringDatabaseNHibernateModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration
            .Modules.AbpNHibernate()
            .FluentConfiguration.Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<AbpBlobStoringDatabaseNHibernateModule>()
            );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
    }
}

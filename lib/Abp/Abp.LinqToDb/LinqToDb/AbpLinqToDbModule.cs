using Abp.Dependency;
using Abp.LinqToDb.Configuration;
using Abp.Modules;
using Abp.Orm;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using LinqToDB;

namespace Abp.LinqToDb;

[DependsOn(typeof(AbpKernelModule))]
public sealed class AbpLinqToDbModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactionScopeAvailable = false;
        IocManager.Register<IAbpLinqToDbModuleConfiguration, AbpLinqToDbModuleConfiguration>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(AbpLinqToDbModule).GetAssembly());

        var configuration = Configuration.Modules.AbpLinqToDb();
        var dataOptions = configuration.DataOptions;

        IocManager.IocContainer.Register(
            Component.For<DataOptions>().Instance(dataOptions).LifeStyle.Singleton
        );

        using var scope = IocManager.CreateScope();
        var additionalOrmRegistrars = scope.ResolveAll<ISecondaryOrmRegistrar>();

        foreach (var registrar in additionalOrmRegistrars)
        {
            // if (registrar.OrmContextKey == "EntityFramework")
            // {
            //     registrar.RegisterRepositories(
            //         IocManager,
            //         EfBasedDapperAutoRepositoryTypes.Default
            //     );
            // }
            //
            // if (registrar.OrmContextKey == "NHibernate")
            // {
            //     registrar.RegisterRepositories(
            //         IocManager,
            //         NhBasedDapperAutoRepositoryTypes.Default
            //     );
            // }
            //
            // if (registrar.OrmContextKey == "EntityFrameworkCore")
            // {
            //     registrar.RegisterRepositories(
            //         IocManager,
            //         EfBasedDapperAutoRepositoryTypes.Default
            //     );
            // }
        }
    }
}

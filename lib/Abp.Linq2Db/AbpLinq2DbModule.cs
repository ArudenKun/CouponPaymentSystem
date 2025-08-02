using Abp.Linq2Db.Common;
using Abp.Linq2Db.Configuration;
using Abp.Modules;
using Abp.Reflection;
using Abp.Reflection.Extensions;

namespace Abp.Linq2Db;

[DependsOn(typeof(AbpLinq2DbCommonModule))]
public class AbpLinq2DbModule : AbpModule
{
    private readonly ITypeFinder _typeFinder;

    public AbpLinq2DbModule(ITypeFinder typeFinder)
    {
        _typeFinder = typeFinder;
    }

    public override void PreInitialize()
    {
        IocManager.Register<IAbpLinq2DbConfiguration, AbpLinq2DbConfiguration>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(AbpLinq2DbModule).GetAssembly());
        // IocManager.IocContainer.Register(
        //     Component
        //         .For(typeof(IDbContextProvider<>))
        //         .ImplementedBy(typeof(UnitOfWorkDbContextProvider<>))
        //         .LifestyleTransient()
        // );
    }
}

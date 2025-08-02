using Abp.Dependency;
using AutoInterfaceAttributes;
using Castle.MicroKernel.Registration;
using LinqToDB.Data;

namespace Abp.Linq2Db.Configuration;

[AutoInterface]
public class AbpLinq2DbConfiguration : IAbpLinq2DbConfiguration
{
    private readonly IIocManager _iocManager;

    public AbpLinq2DbConfiguration(IIocManager iocManager)
    {
        _iocManager = iocManager;
    }

    public void AddDbContext<TDbContext>(Action<AbpDbContextConfiguration<TDbContext>> action)
        where TDbContext : DataConnection
    {
        _iocManager.IocContainer.Register(
            Component
                .For<IAbpDbContextConfigurer<TDbContext>>()
                .Instance(new AbpDbContextConfigurerAction<TDbContext>(action))
                .IsDefault()
        );
    }
}

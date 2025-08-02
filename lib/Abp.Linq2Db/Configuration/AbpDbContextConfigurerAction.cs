using AutoInterfaceAttributes;
using LinqToDB.Data;

namespace Abp.Linq2Db.Configuration;

[AutoInterface(Name = "IAbpDbContextConfigurer")]
public class AbpDbContextConfigurerAction<TDbContext> : IAbpDbContextConfigurer<TDbContext>
    where TDbContext : DataConnection
{
    public Action<AbpDbContextConfiguration<TDbContext>> Action { get; set; }

    public AbpDbContextConfigurerAction(Action<AbpDbContextConfiguration<TDbContext>> action)
    {
        Action = action;
    }

    public void Configure(AbpDbContextConfiguration<TDbContext> configuration)
    {
        Action(configuration);
    }
}

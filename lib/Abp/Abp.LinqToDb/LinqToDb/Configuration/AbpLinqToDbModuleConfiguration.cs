using LinqToDB;

namespace Abp.LinqToDb.Configuration;

internal sealed class AbpLinqToDbModuleConfiguration : IAbpLinqToDbModuleConfiguration
{
    public AbpLinqToDbModuleConfiguration()
    {
        DataOptions = new DataOptions();
    }

    public DataOptions DataOptions { get; }
}

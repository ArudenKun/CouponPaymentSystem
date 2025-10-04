using Abp.Configuration.Startup;

namespace Abp.LinqToDb.Configuration;

public static class AbpLinqToDbModuleConfigurationExtensions
{
    public static IAbpLinqToDbModuleConfiguration AbpLinqToDb(
        this IModuleConfigurations configurations
    )
    {
        return configurations.AbpConfiguration.Get<IAbpLinqToDbModuleConfiguration>();
    }
}

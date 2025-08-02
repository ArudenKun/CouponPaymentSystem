using Abp.Configuration.Startup;

namespace Abp.Linq2Db.Configuration;

public static class AbpLinq2DbConfigurationExtensions
{
    /// <summary>
    /// Used to configure ABP EntityFramework Core module.
    /// </summary>
    public static IAbpLinq2DbConfiguration AbpEfCore(this IModuleConfigurations configurations) =>
        configurations.AbpConfiguration.Get<IAbpLinq2DbConfiguration>();
}

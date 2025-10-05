using System.Collections.Concurrent;
using System.Web.Hosting;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;

namespace Domain.Configuration;

public static class AppConfigurations
{
    private static readonly ConcurrentDictionary<string, IConfigurationRoot> ConfigurationCache;

    static AppConfigurations()
    {
        ConfigurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
    }

    public static IConfigurationRoot Get(string path = "", string? environmentName = null)
    {
        if (path.IsNullOrWhiteSpace() || path.IsNullOrEmpty())
        {
            path = HostingEnvironment.MapPath("~/") ?? AppDomain.CurrentDomain.BaseDirectory;
        }
        var cacheKey = path + "#" + environmentName;
        return ConfigurationCache.GetOrAdd(
            cacheKey,
            _ => BuildConfiguration(path, environmentName)
        );
    }

    private static IConfigurationRoot BuildConfiguration(
        string path,
        string? environmentName = null
    )
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        if (!environmentName.IsNullOrWhiteSpace())
        {
            builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
        }

        return builder.Build();
    }
}

using System.Data;
using System.Data.SqlClient;
using Dapper;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using ZiggyCreatures.Caching.Fusion;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        QuestPDF.Settings.License = LicenseType.Enterprise;

#pragma warning disable CS0618 // Type or member is obsolete
        services.AddTransient<IDbConnection>(_ => new SqlConnection("Data Source=:memory"));
#pragma warning restore CS0618 // Type or member is obsolete

        services.AddDapperInternal();
        services.AddFusionCacheInternal();

        return services;
    }

    private static void AddDapperInternal(this IServiceCollection _)
    {
        var handlers = typeof(IDomain)
            .Assembly.GetExportedTypes()
            .Where(t =>
                !t.IsAbstract
                && t.BaseType is { IsGenericType: true }
                && t.BaseType.GetGenericTypeDefinition() == typeof(SqlMapper.TypeHandler<>)
            )
            .ToDictionary(t => t.BaseType!.GetGenericArguments()[0], x => x);

        foreach (var handler in handlers)
        {
            SqlMapper.AddTypeHandler(
                handler.Key,
                (SqlMapper.ITypeHandler)Activator.CreateInstance(handler.Value)
            );
        }
    }

    private static void AddFusionCacheInternal(this IServiceCollection services)
    {
        services
            .AddFusionCache()
            .WithDefaultEntryOptions(
                new FusionCacheEntryOptions
                {
                    // CACHE DURATION
                    Duration = TimeSpan.FromMinutes(15),
                    // FACTORY TIMEOUTS
                    FactorySoftTimeout = TimeSpan.FromSeconds(10),
                    FactoryHardTimeout = TimeSpan.FromSeconds(30),
                    // AllowTimedOutFactoryBackgroundCompletion = true,
                }
            );
    }
}

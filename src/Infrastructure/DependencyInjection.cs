using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Application.Common.Interfaces.Data;
using Dapper;
using Domain;
using Dommel;
using Infrastructure.Data;
using Infrastructure.Data.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuestPDF.Infrastructure;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using ZiggyCreatures.Caching.Fusion;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        bool isDebug = false
    )
    {
        QuestPDF.Settings.License = LicenseType.Enterprise;

#pragma warning disable CS0618 // Type or member is obsolete
        services.AddTransient<IDbConnection>(_ => new SqlConnection("Data Source=:memory"));
#pragma warning restore CS0618 // Type or member is obsolete
        services.AddTransient<IAppDbContext, AppDbContext>();

        services.AddDommel();
        services.AddFusionCacheInternal();
        services.AddSerilogLogging(isDebug);

        return services;
    }

    private static void AddDommel(this IServiceCollection services)
    {
        DommelMapper.SetKeyPropertyResolver(new AppKeyPropertyResolver());

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

    private const string LogTemplate =
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} {ClassName}] {Message:lj} {NewLine}{Exception}";

    private static void AddSerilogLogging(this IServiceCollection services, bool isDebug = false)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch(
            isDebug ? LogEventLevel.Debug : LogEventLevel.Information
        );

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(loggingLevelSwitch)
            .Enrich.FromLogContext()
            .Enrich.WithClassName()
            .WriteTo.Console(outputTemplate: LogTemplate)
            .CreateLogger();

        services.AddLogging(builder => builder.ClearProviders().AddSerilog(dispose: true));
        services.AddSingleton(loggingLevelSwitch);
    }
}

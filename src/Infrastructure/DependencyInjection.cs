using Application.Common.Interfaces;
using Infrastructure.Data;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.DataProvider.SqlServer;
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
        services.AddSerilogLogging(isDebug);
        services.AddLinqToDBContext<IAppDbContext, AppDbContext>(
            (sp, options) =>
                options
                    .UseSqlServer(
                        "",
                        SqlServerVersion.AutoDetect,
                        SqlServerProvider.SystemDataSqlClient
                    )
                    .UseDefaultLogging(sp)
        );

        services
            .AddFusionCache()
            .WithDefaultEntryOptions(
                new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(15),
                    FactorySoftTimeout = TimeSpan.FromSeconds(10),
                    FactoryHardTimeout = TimeSpan.FromSeconds(30),
                }
            );

        return services;
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

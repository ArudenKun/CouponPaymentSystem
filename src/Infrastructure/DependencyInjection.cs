using System.Data;
using System.Data.SQLite;
using Application.Common.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        QuestPDF.Settings.License = LicenseType.Enterprise;

        services.AddTransient<IAppReadDbContext, AppReadDbContext>();
        services.AddTransient<IAppDbContext, AppDbContext>();
        services.AddTransient<IDbConnection>(_ => new SQLiteConnection("Data Source=:memory"));

        services.AddDapper();

        return services;
    }

    private static void AddDapper(this IServiceCollection _)
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

    // [GenerateServiceRegistrations(
    //     FromAssemblyOf = typeof(IDomain),
    //     Lifetime = ServiceLifetime.Transient,
    //     AsImplementedInterfaces = true,
    //     AssignableTo = typeof(SqlMapper.ITypeHandler),
    //     TypeNameFilter = "*DapperTypeHandler"
    // )]
    // private static partial void AddDapperTypeHandlers(this IServiceCollection services);
}

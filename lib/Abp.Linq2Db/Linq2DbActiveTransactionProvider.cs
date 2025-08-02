using System.Data;
using Abp.Data;
using Abp.Dependency;
using Abp.Linq2Db.Utilities;
using Abp.MultiTenancy;
using LinqToDB.Data;

namespace Abp.Linq2Db;

public class Linq2DbActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
{
    private readonly IIocResolver _iocResolver;

    public Linq2DbActiveTransactionProvider(IIocResolver iocResolver)
    {
        _iocResolver = iocResolver;
    }

    public Task<IDbTransaction> GetActiveTransactionAsync(ActiveTransactionProviderArgs args)
    {
        throw new NotImplementedException();
    }

    public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
    {
        throw new NotImplementedException();
    }

    public Task<IDbConnection> GetActiveConnectionAsync(ActiveTransactionProviderArgs args)
    {
        throw new NotImplementedException();
    }

    public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
    {
        throw new NotImplementedException();
    }

    private async Task<DataConnection> GetDbContextAsync(ActiveTransactionProviderArgs args)
    {
        var dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(
            (Type)args["ContextType"]
        );

        using (
            var dbContextProviderWrapper = _iocResolver.ResolveAsDisposable(dbContextProviderType)
        )
        {
            var method = dbContextProviderWrapper
                .Object.GetType()
                .GetMethod(
                    nameof(IDbContextProvider<>.GetDbContextAsync),
                    [typeof(MultiTenancySides)]
                );

            var result = await ReflectionHelper.InvokeAsync(
                method,
                dbContextProviderWrapper.Object,
                (MultiTenancySides?)args["MultiTenancySide"]
            );

            return (DataConnection)result;
        }
    }

    private DataConnection GetDbContext(ActiveTransactionProviderArgs args)
    {
        var dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(
            (Type)args["ContextType"]
        );

        using (
            var dbContextProviderWrapper = _iocResolver.ResolveAsDisposable(dbContextProviderType)
        )
        {
            var method = dbContextProviderWrapper
                .Object.GetType()
                .GetMethod(
                    nameof(IDbContextProvider<AbpDbContext>.GetDbContext),
                    [typeof(MultiTenancySides)]
                );

            return (DataConnection)
                method.Invoke(
                    dbContextProviderWrapper.Object,
                    [(MultiTenancySides?)args["MultiTenancySide"]]
                );
        }
    }
}

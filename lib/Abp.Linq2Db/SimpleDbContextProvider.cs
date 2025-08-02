using Abp.MultiTenancy;
using LinqToDB.Data;

namespace Abp.Linq2Db;

public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : DataConnection
{
    public TDbContext DbContext { get; }

    public SimpleDbContextProvider(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task<TDbContext> GetDbContextAsync()
    {
        return Task.FromResult(DbContext);
    }

    public Task<TDbContext> GetDbContextAsync(MultiTenancySides? multiTenancySide)
    {
        return Task.FromResult(DbContext);
    }

    public TDbContext GetDbContext()
    {
        return DbContext;
    }

    public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
    {
        return DbContext;
    }
}

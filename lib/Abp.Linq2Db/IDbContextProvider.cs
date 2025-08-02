using Abp.MultiTenancy;
using LinqToDB.Data;

namespace Abp.Linq2Db;

public interface IDbContextProvider<TDbContext>
    where TDbContext : DataConnection
{
    Task<TDbContext> GetDbContextAsync();

    Task<TDbContext> GetDbContextAsync(MultiTenancySides? multiTenancySide);

    TDbContext GetDbContext();

    TDbContext GetDbContext(MultiTenancySides? multiTenancySide);
}

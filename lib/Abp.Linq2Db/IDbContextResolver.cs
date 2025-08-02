using System.Data.Common;
using LinqToDB.Data;

namespace Abp.Linq2Db;

public interface IDbContextResolver
{
    TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
        where TDbContext : DataConnection;
}

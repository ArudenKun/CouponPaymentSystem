using Medallion.Threading.Internal.Data;

namespace Medallion.Threading.SystemSqlServer;

internal static class SqlMultiplexedConnectionLockPool
{
    public static readonly MultiplexedConnectionLockPool Instance = new(
        s => new SqlDatabaseConnection(s)
    );
}

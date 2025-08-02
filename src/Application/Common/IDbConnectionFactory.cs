using System.Data.Common;

namespace CouponPaymentSystem.Application.Common;

public interface IDbConnectionFactory<TConnection>
    where TConnection : DbConnection
{
    IPooledDbConnection<TConnection> Create();
    Task<IPooledDbConnection<TConnection>> CreateAsync();
}

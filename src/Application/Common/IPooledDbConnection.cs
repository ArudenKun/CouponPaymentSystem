using System.Data;
using System.Data.Common;

namespace CouponPaymentSystem.Application.Common;

public interface IPooledDbConnection<out TConnection> : IDbConnection
    where TConnection : DbConnection
{
    public TConnection WrappedConnection { get; }
}

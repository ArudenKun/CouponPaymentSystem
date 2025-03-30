using System.Data;
using Application.Common.Interfaces.Data;
using Domain.Entities;

namespace Infrastructure.Data;

internal class AppDbContext : IAppDbContext
{
    /// <summary>
    ///     DB Connection for internal use
    /// </summary>
    protected IDbConnection InnerConnection { get; }

    /// <summary>
    ///     Constructor
    /// </summary>
    public AppDbContext(IDbConnection connection)
    {
        InnerConnection = connection;
    }

    public virtual IDbConnection Connection
    {
        get
        {
            OpenConnection();
            return InnerConnection;
        }
    }

    public void OpenConnection()
    {
        if (
            InnerConnection.State != ConnectionState.Open
            && InnerConnection.State != ConnectionState.Connecting
        )
            InnerConnection.Open();
    }

    public virtual IDbTransaction BeginTransaction()
    {
        return Connection.BeginTransaction();
    }

    /// <summary>
    ///     Close DB connection
    /// </summary>
    public void Dispose()
    {
        if (InnerConnection.State != ConnectionState.Closed)
            InnerConnection.Close();
    }

    private IRepository<Transaction>? _transactions;

    public IRepository<Transaction> Transactions =>
        _transactions ??= new Repository<Transaction>(Connection);
}

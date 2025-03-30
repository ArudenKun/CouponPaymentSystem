using System.Collections.Concurrent;
using System.Data;
using Application.Common.Interfaces.Data;
using Domain.Common.Entities;

namespace Infrastructure.Data;

internal abstract class AppDbContextBase : IAppDbContextBase
{
    /// <summary>
    ///     DB Connection for internal use
    /// </summary>
    protected IDbConnection InnerConnection { get; }

    /// <summary>
    ///     Stores repository instances by their entity type
    /// </summary>
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    /// <summary>
    ///     Constructor
    /// </summary>
    public AppDbContextBase(IDbConnection connection)
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
        {
            InnerConnection.Open();
        }
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

        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Gets or creates a repository for the given entity type
    /// </summary>
    protected IRepository<TEntity> Set<TEntity>()
        where TEntity : class, IEntity =>
        (IRepository<TEntity>)
            _repositories.GetOrAdd(typeof(TEntity), _ => new Repository<TEntity>(InnerConnection));
}

using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using Abp;
using Castle.Core.Logging;
using CouponPaymentSystem.Application.Common;

namespace CouponPaymentSystem.Infrastructure.Services;

internal sealed class DbConnectionFactory<TConnection>
    : IDbConnectionFactory<TConnection>,
        IShouldInitialize,
        IDisposable
    where TConnection : DbConnection
{
    private const int InitialConcurrency = 4;
    private readonly ConcurrentBag<TConnection> _connections = [];
    private readonly ILogger _logger;
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString, ILogger logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public void Initialize()
    {
        _logger.TraceFormat(
            "Creating {InitialConnections} initial connections in the pool",
            InitialConcurrency
        );
        for (var i = 0; i < InitialConcurrency; ++i)
        {
            _logger.TraceFormat("Opening connection to {ConnectionString}", _connectionString);
            _connections.Add(CreateConnection());
        }
    }

    public IPooledDbConnection<TConnection> Create()
    {
        if (_connections.TryTake(out var db))
            return new PooledDbConnection(db, this);

        _logger.Trace("Adding a new connection to the connection pool");
        _logger.TraceFormat("Opening connection to {ConnectionString}", _connectionString);
        db = CreateConnection();
        return new PooledDbConnection(db, this);
    }

    public async Task<IPooledDbConnection<TConnection>> CreateAsync()
    {
        if (_connections.TryTake(out var db))
            return new PooledDbConnection(db, this);

        _logger.Trace("Adding a new connection to the connection pool");
        _logger.TraceFormat("Opening connection to {ConnectionString}", _connectionString);
        db = await CreateConnectionAsync();
        return new PooledDbConnection(db, this);
    }

    public void Dispose()
    {
        foreach (var conn in _connections)
        {
            _logger.TraceFormat("Closing connection to {ConnectionString}", _connectionString);
            conn.Close();
            conn.Dispose();
        }
    }

    private TConnection CreateConnection()
    {
        var conn = (TConnection)Activator.CreateInstance(typeof(TConnection), _connectionString);
        conn.Open();
        return conn;
    }

    private async Task<TConnection> CreateConnectionAsync()
    {
        var conn = (TConnection)Activator.CreateInstance(typeof(TConnection), _connectionString);
        await conn.OpenAsync();
        return conn;
    }

    public sealed class PooledDbConnection(
        TConnection connection,
        DbConnectionFactory<TConnection> factory
    ) : IPooledDbConnection<TConnection>
    {
        public void Dispose() => factory._connections.Add(connection);

        public TConnection WrappedConnection => connection;

        public IDbTransaction BeginTransaction() => connection.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel) =>
            connection.BeginTransaction(isolationLevel);

        public void Close() => connection.Close();

        public void ChangeDatabase(string databaseName) => connection.ChangeDatabase(databaseName);

        public IDbCommand CreateCommand() => connection.CreateCommand();

        public void Open() => connection.Open();

        public string ConnectionString
        {
            get => connection.ConnectionString;
            set => connection.ConnectionString = value;
        }

        public int ConnectionTimeout => connection.ConnectionTimeout;
        public string Database => connection.Database;
        public ConnectionState State => connection.State;
    }
}

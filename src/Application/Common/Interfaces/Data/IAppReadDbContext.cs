using System.Data;

namespace Application.Common.Interfaces.Data;

public interface IAppReadDbContext : IDisposable
{
    IDbConnection Connection { get; }

    /// <summary>Open DB connection</summary>
    void OpenConnection();

    /// <summary>Open DB connection and Begin transaction</summary>
    IDbTransaction BeginTransaction();
}

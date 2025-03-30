using System.Data;
using Domain.Entities;

namespace Application.Common.Interfaces.Data;

public interface IAppDbContext : IDisposable
{
    IDbConnection Connection { get; }

    /// <summary>Open DB connection</summary>
    void OpenConnection();

    /// <summary>Open DB connection and Begin transaction</summary>
    IDbTransaction BeginTransaction();

    IRepository<Transaction> Transactions { get; }
}

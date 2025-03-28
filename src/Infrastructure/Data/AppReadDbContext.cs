using System.Data;
using Application.Common.Interfaces;
using Domain.Entities;
using MicroOrm.Dapper.Repositories.DbContext;

namespace Infrastructure.Data;

internal class AppReadDbContext : DapperDbContext, IAppReadDbContext
{
    private IReadRepository<TransactionId, Transaction>? _readTransactions;

    public AppReadDbContext(IDbConnection connection)
        : base(connection) { }

    public IReadRepository<TransactionId, Transaction> ReadTransactions =>
        _readTransactions ??= new ReadRepository<TransactionId, Transaction>(Connection);
}

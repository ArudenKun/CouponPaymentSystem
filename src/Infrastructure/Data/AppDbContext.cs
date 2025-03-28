using System.Data;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Data;

internal sealed class AppDbContext : AppReadDbContext, IAppDbContext
{
    private IRepository<TransactionId, Transaction>? _transactionRepository;

    public AppDbContext(IDbConnection connection)
        : base(connection) { }

    public IRepository<TransactionId, Transaction> Transactions =>
        _transactionRepository ??= new Repository<TransactionId, Transaction>(Connection);
}

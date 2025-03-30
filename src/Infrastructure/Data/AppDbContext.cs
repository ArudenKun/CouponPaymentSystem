using System.Data;
using Application.Common.Interfaces.Data;
using Domain.Entities;

namespace Infrastructure.Data;

internal sealed class AppDbContext : AppDbContextBase, IAppDbContext
{
    public AppDbContext(IDbConnection connection)
        : base(connection) { }

    public IRepository<Transaction> Transactions => Set<Transaction>();
}

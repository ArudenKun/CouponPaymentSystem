using Application.Common.Interfaces;
using Domain.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure.Data;

internal sealed class AppDbContext : DataConnection, IAppDbContext
{
    public AppDbContext(DataOptions<AppDbContext> options)
        : base(options.Options) { }

    public ITable<Transaction> Transactions => this.GetTable<Transaction>();
}

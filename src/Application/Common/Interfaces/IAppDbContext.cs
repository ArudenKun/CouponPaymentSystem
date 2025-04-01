using Domain.Entities;
using LinqToDB;

namespace Application.Common.Interfaces;

public interface IAppDbContext : IDataContext
{
    ITable<Transaction> Transactions { get; }
}

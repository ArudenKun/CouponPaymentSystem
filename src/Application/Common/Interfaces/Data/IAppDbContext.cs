using Domain.Entities;

namespace Application.Common.Interfaces.Data;

public interface IAppDbContext : IAppDbContextBase
{
    IRepository<Transaction> Transactions { get; }
}

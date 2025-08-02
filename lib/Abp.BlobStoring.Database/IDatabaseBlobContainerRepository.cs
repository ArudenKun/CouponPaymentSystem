using Abp.Domain.Repositories;

namespace Abp.BlobStoring.Database;

public interface IDatabaseBlobContainerRepository : IRepository<DatabaseBlobContainer, Guid>
{
    Task<DatabaseBlobContainer?> FindAsync(string name);
}

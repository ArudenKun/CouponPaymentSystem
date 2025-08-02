using Abp.Domain.Repositories;

namespace Abp.BlobStoring.Database;

public interface IDatabaseBlobRepository : IRepository<DatabaseBlob, Guid>
{
    Task<DatabaseBlob?> FindAsync(Guid containerId, string name);

    Task<bool> ExistsAsync(Guid containerId, string name);

    Task<bool> DeleteAsync(Guid containerId, string name);
}

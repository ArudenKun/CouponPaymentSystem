using Abp.NHibernate;
using Abp.NHibernate.Repositories;

namespace Abp.BlobStoring.Database.NHibernate;

public class NHibernateDatabaseBlobRepository
    : NhRepositoryBase<DatabaseBlob, Guid>,
        IDatabaseBlobRepository
{
    public NHibernateDatabaseBlobRepository(ISessionProvider sessionProvider)
        : base(sessionProvider) { }

    public async Task<DatabaseBlob?> FindAsync(Guid containerId, string name) =>
        await FirstOrDefaultAsync(x => x.ContainerId == containerId && x.Name == name);

    public async Task<bool> ExistsAsync(Guid containerId, string name)
    {
        var count = await CountAsync(x => x.ContainerId == containerId && x.Name == name);
        return count > 0;
    }

    public async Task<bool> DeleteAsync(Guid containerId, string name)
    {
        var blob = await FindAsync(containerId, name);
        if (blob is null)
        {
            return false;
        }
        await DeleteAsync(blob);
        return true;
    }
}

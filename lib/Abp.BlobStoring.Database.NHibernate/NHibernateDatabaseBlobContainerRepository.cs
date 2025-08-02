using Abp.NHibernate;
using Abp.NHibernate.Repositories;

namespace Abp.BlobStoring.Database.NHibernate;

public class NHibernateDatabaseBlobContainerRepository
    : NhRepositoryBase<DatabaseBlobContainer, Guid>,
        IDatabaseBlobContainerRepository
{
    public NHibernateDatabaseBlobContainerRepository(ISessionProvider sessionProvider)
        : base(sessionProvider) { }

    public async Task<DatabaseBlobContainer?> FindAsync(string name) =>
        await FirstOrDefaultAsync(x => x.Name == name);
}

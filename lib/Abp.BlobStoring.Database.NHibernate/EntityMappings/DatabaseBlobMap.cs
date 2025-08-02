using Abp.NHibernate.EntityMappings;

namespace Abp.BlobStoring.Database.NHibernate.EntityMappings;

public class DatabaseBlobMap : EntityMap<DatabaseBlob, Guid>
{
    public DatabaseBlobMap()
        : base(nameof(DatabaseBlob))
    {
        Id(x => x.Id).GeneratedBy.Assigned();
        Map(x => x.TenantId).Nullable();
        Map(x => x.ContainerId).Not.Nullable();
        Map(x => x.Name).Not.Nullable().Length(DatabaseBlobConstants.MaxNameLength);
        Map(x => x.Content).Not.Nullable().Length(DatabaseBlobConstants.MaxContentLength);
    }
}

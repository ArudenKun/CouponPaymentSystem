using Abp.NHibernate.EntityMappings;

namespace Abp.BlobStoring.Database.NHibernate.EntityMappings;

public class DatabaseBlobContainerMap : EntityMap<DatabaseBlobContainer, Guid>
{
    public DatabaseBlobContainerMap()
        : base(nameof(DatabaseBlobContainer))
    {
        Id(x => x.Id);
        Map(x => x.TenantId).Nullable();
        Map(x => x.Name).Not.Nullable().Length(DatabaseContainerConstants.MaxNameLength);
    }
}

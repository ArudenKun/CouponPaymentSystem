using System.Diagnostics.CodeAnalysis;
using Abp.Domain.Entities;

namespace Abp.BlobStoring.Database;

public class DatabaseBlobContainer : AggregateRoot<Guid>, IMayHaveTenant
{
    public DatabaseBlobContainer() { }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public DatabaseBlobContainer(Guid id, string name, Guid? tenantId = null)
    {
        Id = id;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        TenantId = tenantId;
    }

    public virtual Guid? TenantId { get; set; }

    public virtual string Name { get; protected set; } = string.Empty;
}

using System.Diagnostics.CodeAnalysis;
using Abp.Auditing;
using Abp.Domain.Entities;

namespace Abp.BlobStoring.Database;

public class DatabaseBlob : AggregateRoot<Guid>, IMayHaveTenant
{
    public DatabaseBlob() { }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public DatabaseBlob(
        Guid id,
        Guid containerId,
        string name,
        byte[] content,
        int? tenantId = null
    )
    {
        Id = id;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        ContainerId = containerId;
        Content = CheckContentLength(content);
        TenantId = tenantId;
    }

    public virtual Guid ContainerId { get; protected set; }

    public virtual int? TenantId { get; set; }

    public virtual string Name { get; protected set; } = string.Empty;

    [DisableAuditing]
    public virtual byte[] Content { get; protected set; } = [];

    public virtual void SetContent(byte[] content)
    {
        Content = CheckContentLength(content);
    }

    protected virtual byte[] CheckContentLength(byte[] content)
    {
        Check.NotNull(content, nameof(content));

        if (content.Length >= DatabaseBlobConstants.MaxContentLength)
        {
            throw new AbpException(
                $"Blob content size cannot be more than {DatabaseBlobConstants.MaxContentLength} Bytes."
            );
        }

        return content;
    }
}

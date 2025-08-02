using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.IO.Extensions;
using Abp.Runtime.Session;

namespace Abp.BlobStoring.Database;

public class DatabaseBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IDatabaseBlobRepository DatabaseBlobRepository { get; }
    protected IDatabaseBlobContainerRepository DatabaseBlobContainerRepository { get; }
    protected IAbpSession AbpSession { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public DatabaseBlobProvider(
        IDatabaseBlobRepository databaseBlobRepository,
        IDatabaseBlobContainerRepository databaseBlobContainerRepository,
        IAbpSession abpSession,
        IGuidGenerator guidGenerator
    )
    {
        DatabaseBlobRepository = databaseBlobRepository;
        DatabaseBlobContainerRepository = databaseBlobContainerRepository;
        AbpSession = abpSession;
        GuidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var container = await GetOrCreateContainerAsync(args.ContainerName);

        var blob = await DatabaseBlobRepository.FindAsync(container.Id, args.BlobName);

        var content = await args.BlobStream.GetAllBytesAsync(args.CancellationToken);

        if (blob != null)
        {
            if (!args.OverrideExisting)
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten."
                );
            }

            blob.SetContent(content);
            await DatabaseBlobRepository.UpdateAsync(blob);
        }
        else
        {
            blob = new DatabaseBlob(
                GuidGenerator.Create(),
                container.Id,
                args.BlobName,
                content,
                AbpSession.TenantId
            );
            await DatabaseBlobRepository.InsertAsync(blob);
        }
    }

    [UnitOfWork]
    public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var container = await DatabaseBlobContainerRepository.FindAsync(args.ContainerName);

        if (container == null)
        {
            return false;
        }

        return await DatabaseBlobRepository.DeleteAsync(container.Id, args.BlobName);
    }

    [UnitOfWork(isTransactional: false)]
    public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var container = await DatabaseBlobContainerRepository.FindAsync(args.ContainerName);

        if (container == null)
        {
            return false;
        }

        return await DatabaseBlobRepository.ExistsAsync(container.Id, args.BlobName);
    }

    [UnitOfWork(isTransactional: false)]
    public override async Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var container = await DatabaseBlobContainerRepository.FindAsync(args.ContainerName);

        if (container == null)
        {
            return null;
        }

        var blob = await DatabaseBlobRepository.FindAsync(container.Id, args.BlobName);

        if (blob == null)
        {
            return null;
        }

        return new MemoryStream(blob.Content);
    }

    [UnitOfWork]
    protected virtual async Task<DatabaseBlobContainer> GetOrCreateContainerAsync(string name)
    {
        var container = await DatabaseBlobContainerRepository.FindAsync(name);
        if (container != null)
        {
            return container;
        }

        container = new DatabaseBlobContainer(GuidGenerator.Create(), name, AbpSession.TenantId);
        await DatabaseBlobContainerRepository.InsertAsync(container);
        return container;
    }
}

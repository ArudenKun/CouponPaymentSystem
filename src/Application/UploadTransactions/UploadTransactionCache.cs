using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using AutoInterfaceAttributes;
using CouponPaymentSystem.Application.UploadTransactions.Dto;
using CouponPaymentSystem.Core.UploadTransactions;

namespace CouponPaymentSystem.Application.UploadTransactions;

[AutoInterface(Inheritance = [typeof(IEntityCache<UploadTransactionDto>)])]
public class UploadTransactionCache
    : EntityCache<UploadTransaction, UploadTransactionDto>,
        IUploadTransactionCache,
        ITransientDependency
{
    public UploadTransactionCache(
        ICacheManager cacheManager,
        IRepository<UploadTransaction, int> repository,
        IUnitOfWorkManager unitOfWorkManager,
        string? cacheName = null
    )
        : base(cacheManager, repository, unitOfWorkManager, cacheName) { }
}

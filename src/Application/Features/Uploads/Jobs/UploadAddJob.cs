using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using CouponPaymentSystem.Domain.Entities;
using CouponPaymentSystem.Domain.Enums;
using NHibernate.Linq;

namespace CouponPaymentSystem.Application.Features.Uploads.Jobs;

public class UploadAddJob : AsyncBackgroundJob<UploadAddJobInput>, ITransientDependency
{
    private readonly IRepository<Upload> _uploadRepository;

    public UploadAddJob(IRepository<Upload> uploadRepository)
    {
        _uploadRepository = uploadRepository;
    }

    public override async Task ExecuteAsync(UploadAddJobInput args)
    {
        await _uploadRepository.Query(query =>
            query
                .Where(s => s.Id == args.UploadId)
                .UpdateBuilder()
                .Set(x => x.Status, Status.Ready)
                .UpdateAsync()
        );
    }
}

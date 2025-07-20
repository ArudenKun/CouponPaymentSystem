using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using CouponPaymentSystem.Core.Uploads;
using NHibernate.Linq;

namespace CouponPaymentSystem.Application.Uploads;

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
                .Set(x => x.Status, UploadStatus.Finished)
                .UpdateAsync()
        );
    }
}

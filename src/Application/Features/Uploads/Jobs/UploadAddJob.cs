using System.Threading.Tasks.Dataflow;
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
        var entries = new List<UploadEntry>();
        var uploads = new List<Upload>();
        var tb = new TransformBlock<UploadEntry, Upload>(entry => (Upload)new object());

        foreach (var entry in entries)
        {
            await tb.SendAsync(entry);
        }

        for (var i = 0; i < entries.Count; i++)
        {
            uploads.Add(await tb.ReceiveAsync());
        }

        tb.Complete();
        await tb.Completion;

        if (tb.Completion.Exception != null)
        {
            throw tb.Completion.Exception;
        }

        await _uploadRepository.Query(query =>
            query
                .Where(s => s.Id == args.UploadId)
                .UpdateBuilder()
                .Set(x => x.Status, Status.Ready)
                .UpdateAsync()
        );
    }

    public sealed record UploadEntry(string AccountNumber, string AccountName);
}

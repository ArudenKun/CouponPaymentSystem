using Abp.Dependency;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Hangfire;

namespace Abp.Hangfire.Recurring;

public class HangfireBackgroundWorkerManager : RunnableBase, IBackgroundWorkerManager, IDisposable
{
    private readonly IIocResolver _iocResolver;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly List<IBackgroundWorker> _backgroundJobs;

    public HangfireBackgroundWorkerManager(
        IIocResolver iocResolver,
        IRecurringJobManager recurringJobManager
    )
    {
        _iocResolver = iocResolver;
        _recurringJobManager = recurringJobManager;
        _backgroundJobs = [];
    }

    public void Add(IBackgroundWorker worker)
    {
        switch (worker)
        {
            case IRecurringJob recurringJob:
                _recurringJobManager.AddOrUpdate(
                    recurringJob.JobId,
                    () => recurringJob.Execute(),
                    () => recurringJob.Cron
                );
                break;
            case IAsyncRecurringJob asyncRecurringJob:
                _recurringJobManager.AddOrUpdate(
                    asyncRecurringJob.JobId,
                    () => asyncRecurringJob.ExecuteAsync(),
                    () => asyncRecurringJob.Cron
                );
                break;
        }

        _backgroundJobs.Add(worker);

        if (IsRunning)
        {
            worker.Start();
        }
    }

    public override void Start()
    {
        base.Start();

        _backgroundJobs.ForEach(job => job.Start());
    }

    public override void Stop()
    {
        _backgroundJobs.ForEach(job => job.Stop());

        base.Stop();
    }

    public override void WaitToStop()
    {
        _backgroundJobs.ForEach(job => job.WaitToStop());

        base.WaitToStop();
    }

    private bool _isDisposed;

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        _backgroundJobs.ForEach(_iocResolver.Release);
        _backgroundJobs.Clear();
    }
}

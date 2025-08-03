using Abp.Threading.BackgroundWorkers;

namespace Abp.Hangfire.Recurring;

public interface IRecurringJobBase : IBackgroundWorker
{
    abstract string JobId { get; }
    abstract string Cron { get; }
}

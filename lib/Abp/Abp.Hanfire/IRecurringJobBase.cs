using Abp.Threading.BackgroundWorkers;

namespace Abp;

public interface IRecurringJobBase : IBackgroundWorker
{
    abstract string Id { get; }
    abstract string CronSchedule { get; }
}

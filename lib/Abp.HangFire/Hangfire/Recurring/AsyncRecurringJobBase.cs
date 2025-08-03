using Abp.Threading.BackgroundWorkers;
using AutoInterfaceAttributes;

namespace Abp.Hangfire.Recurring;

[AutoInterface(
    Name = "IAsyncRecurringJob",
    Inheritance = [typeof(IBackgroundWorker), typeof(IRecurringJobBase)]
)]
public abstract class AsyncRecurringJobBase : BackgroundWorkerBase, IAsyncRecurringJob
{
    public abstract string JobId { get; }
    public abstract string Cron { get; }
    public abstract Task ExecuteAsync();
}

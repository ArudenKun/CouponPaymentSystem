using Abp.Threading.BackgroundWorkers;
using AutoInterfaceAttributes;

namespace Abp.Hangfire.Recurring;

[AutoInterface(
    Name = "IRecurringJob",
    Inheritance = [typeof(IBackgroundWorker), typeof(IRecurringJobBase)]
)]
public abstract class RecurringJobBase : BackgroundWorkerBase, IRecurringJob
{
    public abstract string JobId { get; }
    public abstract string Cron { get; }
    public abstract void Execute();
}

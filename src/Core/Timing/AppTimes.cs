using Abp.Dependency;

namespace CouponPaymentSystem.Core.Timing;

public class AppTimes : ISingletonDependency
{
    /// <summary>
    /// Gets the startup time of the application.
    /// </summary>
    public DateTime StartupTime { get; internal set; }
}

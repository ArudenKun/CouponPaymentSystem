using Abp.Application.Services;
using CouponPaymentSystem.Core.Configuration.Options;

namespace CouponPaymentSystem.Application;

public abstract class CpsAppServiceBase : ApplicationService
{
    public required CpsOptions CpsOptions { get; init; }
}

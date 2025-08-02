using Abp.Application.Services;
using CouponPaymentSystem.Application.Common.Configurations;

namespace CouponPaymentSystem.Application;

public abstract class CpsAppServiceBase : ApplicationService
{
    public required IApplicationSettings Settings { get; init; }
}

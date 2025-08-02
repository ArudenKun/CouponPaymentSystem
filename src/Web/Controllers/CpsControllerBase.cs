using Abp.Web.Mvc.Controllers;
using CouponPaymentSystem.Application.Common.Configurations;

namespace CouponPaymentSystem.Controllers;

// [Authorize]
public abstract class CpsControllerBase : AbpController
{
    public required IApplicationSettings Settings { protected get; init; }
}

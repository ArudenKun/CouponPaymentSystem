using System.Web.Mvc;
using Abp.Web.Mvc.Controllers;
using CouponPaymentSystem.Core.Configuration.Options;

namespace CouponPaymentSystem.Controllers;

// [Authorize]
public abstract class CpsControllerBase : AbpController
{
    public required CpsOptions CpsOptions { protected get; init; }
}

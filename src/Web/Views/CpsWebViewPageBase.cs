using Abp.Web.Mvc.Views;

namespace CouponPaymentSystem.Views;

public abstract class CpsWebViewPageBase : CpsWebViewPageBase<dynamic>;

public abstract class CpsWebViewPageBase<TModel> : AbpWebViewPage<TModel>;

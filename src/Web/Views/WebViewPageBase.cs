using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Views;

namespace Web.Views;

public abstract class WebViewPageBase : WebViewPageBase<dynamic>;

public abstract class WebViewPageBase<TModel> : AbpWebViewPage<TModel>
{
    public IAbpSession AbpSession { get; init; } = NullAbpSession.Instance;
}

using System.Web.Mvc;
using Abp.Runtime.Session;

namespace Web.Views;

public abstract class WebViewPageBase : WebViewPageBase<dynamic>;

public abstract class WebViewPageBase<TModel> : WebViewPage<TModel>
{
    public IAbpSession AbpSession { get; init; } = NullAbpSession.Instance;
}

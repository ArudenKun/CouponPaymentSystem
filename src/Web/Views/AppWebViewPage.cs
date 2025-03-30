using System.Web.Mvc;
using Application.Common;

namespace Web.Views;

public abstract class AppWebViewPage : WebViewPage
{
    private AppPrincipal? _user;

    public new AppPrincipal User => _user ??= new AppPrincipal(base.User);
}

public abstract class AppWebViewPage<TModel> : WebViewPage<TModel>
{
    private AppPrincipal? _user;

    public new AppPrincipal User => _user ??= new AppPrincipal(base.User);
}

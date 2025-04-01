using System.Web.Mvc;
using Application.Common;
using Htmx;

namespace Web.Controllers.Common;

[Authorize]
public abstract class AppControllerBase : Controller
{
    private AppPrincipal? _user;
    private readonly Lazy<bool> _isAuthenticated;

    protected AppControllerBase()
    {
        _isAuthenticated = new Lazy<bool>(() => User.Identity?.IsAuthenticated ?? false);
    }

    public new AppPrincipal User => _user ??= new AppPrincipal(base.User);

    public bool IsAuthenticated => _isAuthenticated.Value;

    protected bool IsLocalUrl(string url) => Url.IsLocalUrl(url);

    protected ActionResult RedirectToLocal(string returnUrl) =>
        Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToHome();

    protected ActionResult RedirectToReferrer()
    {
        var referrer = Request.UrlReferrer?.ToString();

        if (string.IsNullOrWhiteSpace(referrer) || !Url.IsLocalUrl(referrer))
        {
            return RedirectToAction("Index", "Home");
        }

        return Redirect(referrer);
    }

    protected ActionResult RedirectToHome() => RedirectToAction("Index", "Home");

    protected JsonResult JsonGet(object data) => Json(data, JsonRequestBehavior.AllowGet);

    protected static ActionResult Empty() => new EmptyResult();

    protected ActionResult HtmxView(object model) => HtmxView(null, model);

    protected ActionResult HtmxView(string? viewName = null, object? model = null)
    {
        var isHtmxRequest = Request.IsHtmx();

        if (model is null)
        {
            return isHtmxRequest ? PartialView(viewName) : View(viewName);
        }

        if (!string.IsNullOrEmpty(viewName) || !string.IsNullOrWhiteSpace(viewName))
        {
            return isHtmxRequest ? PartialView(viewName, model) : View(viewName, model);
        }

        return isHtmxRequest ? PartialView(model) : View(model);
    }
}

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

    protected ActionResult RedirectToLocal(string returnUrl)
    {
        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToHome();
    }

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

    protected JsonResult JsonGet(object data)
    {
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    protected ActionResult Empty()
    {
        return new EmptyResult();
    }

    protected ActionResult HtmxView()
    {
        if (Request.IsHtmx())
        {
            return PartialView();
        }

        return View();
    }

    protected ActionResult HtmxView<TModel>(TModel model)
    {
        if (Request.IsHtmx())
        {
            return PartialView(model);
        }

        return View(model);
    }
}

using System.Web.Mvc;
using Application.Common;

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

    protected JsonResult JsonGet(object data)
    {
        return Json(data, JsonRequestBehavior.AllowGet);
    }
}

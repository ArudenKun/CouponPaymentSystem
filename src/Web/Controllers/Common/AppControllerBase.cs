using System.Web.Mvc;
using Application;

namespace Web.Controllers.Common;

[Authorize]
public abstract class AppControllerBase : Controller
{
    private AppPrincipal? _user;

    public new AppPrincipal User => _user ??= new AppPrincipal(base.User);

    private bool IsAuthenticated => User.Identity.IsAuthenticated;

    protected ActionResult RedirectToLocal(string returnUrl) =>
        Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : Request.UrlReferrer?.ToString());

    protected ActionResult RedirectToReferrer()
    {
        var referrer = Request.UrlReferrer;
        if (referrer is null)
        {
            return RedirectToAction("Index", "Home");
        }

        return Redirect(referrer.ToString());
    }
}

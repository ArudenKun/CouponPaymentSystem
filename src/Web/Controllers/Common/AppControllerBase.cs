using System.Web.Mvc;
using Application;
using Application.Common;

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

    protected ActionResult JsonGet(object data)
    {
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // var authTicket = ((ClaimsIdentity)User.Identity)?.BootstrapContext as ClaimsIdentity;
        // if (authTicket?.FindFirst("Expires")?.Value != null)
        // {
        //     var expires = DateTime.Parse(authTicket.FindFirst("Expires").Value);
        //     if (DateTime.UtcNow > expires)
        //     {
        //         HttpContext
        //             .GetOwinContext()
        //             .Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        //         filterContext.Result = RedirectToAction("Login", "Account");
        //     }
        // }
        base.OnActionExecuting(filterContext);
    }
}

using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Web.Controllers;

// [RoutePrefix("auth")]
public class AuthController : Controller
{
    private IAuthenticationManager AuthenticationManager =>
        HttpContext.GetOwinContext().Authentication;

    // [Route("login")]
    public ActionResult Login(string returnUrl = "")
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "Test"),
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, "Admin"),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1),
        };
        AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        AuthenticationManager.SignIn(authProperties, identity);
        return RedirectToLocal(returnUrl);
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
        return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "Home"));
    }
}

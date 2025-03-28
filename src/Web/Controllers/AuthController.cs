﻿using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Web.Controllers;

public class AuthController : Controller
{
    public AuthController() { }

    private IAuthenticationManager AuthenticationManager =>
        HttpContext.GetOwinContext().Authentication;

    [HttpGet]
    public ActionResult Login(string returnUrl = "")
    {
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationType);
        identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
        AuthenticationManager.SignIn(identity);
        return RedirectToLocal(returnUrl);
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
        return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "Home"));
    }
}

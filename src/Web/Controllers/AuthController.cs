using System.Security.Claims;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Bogus;
using Domain.Common.Enums;
using Domain.Entities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;
using Web.Controllers.Common;

namespace Web.Controllers;

[RoutePrefix("auth")]
[AllowAnonymous]
public class AuthController : AppControllerBase
{
    private IAuthenticationManager AuthenticationManager =>
        HttpContext.GetOwinContext().Authentication;

    [Route("login")]
    public ActionResult Login([QueryString] string returnUrl = "", [QueryString] int sessionId = 0)
    {
        if (sessionId is 0)
        {
            sessionId = new Faker().Random.Int();
        }

        var fakeUser = new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid().ToString())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.MiddleName, f => f.Lorem.Letter())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.AppRoleName, f => f.System.MimeType())
            .RuleFor(u => u.AppRoleId, f => f.Random.Guid().ToString())
            .Generate();

        var userDataJson = JsonConvert.SerializeObject(fakeUser);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, fakeUser.FullName),
            new(ClaimTypes.NameIdentifier, fakeUser.Id),
            new(ClaimTypes.Role, "Admin"),
            new(ClaimTypes.UserData, userDataJson),
            new(DomainClaimTypes.SessionId, sessionId.ToString()),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
        };
        AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        AuthenticationManager.SignIn(authProperties, identity);
        return RedirectToLocal(returnUrl);
    }

    [Route("logout")]
    public ActionResult Logout()
    {
        throw new NotImplementedException();
    }
}

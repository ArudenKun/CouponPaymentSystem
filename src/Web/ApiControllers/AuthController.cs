using System.Web.Http;
using System.Web.ModelBinding;

namespace Web.ApiControllers;

public class AuthController : ApiController
{
    [ApiHttpGet]
    public IHttpActionResult Login(
        [QueryString] string username = "UNKNOWN",
        [QueryString] string password = "UNKNOWN"
    )
    {
        return Json(new { username, password });
    }
}

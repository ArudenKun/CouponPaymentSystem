using System.Web.Mvc;
using Web.Controllers.Common;

namespace Web.Controllers;

public class HomeController : AppControllerBase
{
    [AllowAnonymous]
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Upload()
    {
        ViewBag.Message = "Your application description page.";

        return View();
    }

    [AllowAnonymous]
    public ActionResult History()
    {
        ViewBag.Message = "Your contact page.";

        return View();
    }

    [AllowAnonymous]
    public ActionResult Login()
    {
        return View();
    }
}

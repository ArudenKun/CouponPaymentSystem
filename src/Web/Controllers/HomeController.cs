using System.Web.Mvc;
using Web.Controllers.Common;

namespace Web.Controllers;

[AllowAnonymous]
public class HomeController : AppControllerBase
{
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult About()
    {
        ViewBag.Message = "Your application description page.";

        return View();
    }

    public ActionResult Contact()
    {
        ViewBag.Message = "Your contact page.";

        return View();
    }
}

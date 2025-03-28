using System.Web.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Upload()
    {
        ViewBag.Message = "Your application description page.";

        return View();
    }

    public ActionResult History()
    {
        ViewBag.Message = "Your contact page.";

        return View();
    }
}

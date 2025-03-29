using System.Web.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [Authorize]
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

    public ActionResult TestJson()
    {
        return Json(
            new
            {
                Message = "Hello World!",
                Text = "Test",
                Yeet = "ASDASDAS",
            },
            JsonRequestBehavior.AllowGet
        );
    }

    public ActionResult Login()
    {
        return View();
    }
}

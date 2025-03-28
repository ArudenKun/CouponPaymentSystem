using System.Web.Mvc;
using Application.Common.Interfaces;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IAppDbContext _appDbContext;

    public HomeController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

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

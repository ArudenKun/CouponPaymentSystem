using System.Web.Mvc;
using CouponPaymentSystem.Application.UploadTransactions;

namespace CouponPaymentSystem.Controllers;

public class HomeController : CpsControllerBase
{
    public HomeController() { }

    public ActionResult Index()
    {
        if (CpsOptions is not null)
        {
            Logger.Info($"Testing: {CpsOptions.Checker}");
        }

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

using System.Web.Mvc;
using Abp.Extensions;
using NHibernate;
using NHibernate.Transaction;

namespace CouponPaymentSystem.Controllers;

public class HomeController : CpsControllerBase
{
    public HomeController() { }

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

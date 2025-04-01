using System.Web.Mvc;
using Htmx;
using Web.Controllers.Common;
using Web.ViewModels;

namespace Web.Controllers;

public class HomeController : AppControllerBase
{
    [AllowAnonymous]
    public ActionResult Index()
    {
        return HtmxView();
    }

    public ActionResult Upload()
    {
        ViewBag.Message = "Your application description page.";

        return HtmxView();
    }

    [AllowAnonymous]
    public ActionResult History()
    {
        ViewBag.Message = "Your contact page.";
        var viewModel = new HistoryViewModel();
        return HtmxView(viewModel);
    }

    [AllowAnonymous]
    public ActionResult Login()
    {
        return HtmxView();
    }
}

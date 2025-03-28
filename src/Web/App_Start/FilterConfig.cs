using System.Web.Mvc;
using Web.Controllers.Common;

namespace Web;

public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
        filters.Add(new JsonNetResultOverrideAttribute());
    }
}

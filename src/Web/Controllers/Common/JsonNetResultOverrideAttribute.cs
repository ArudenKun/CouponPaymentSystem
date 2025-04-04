﻿using System.Web.Mvc;

namespace Web.Controllers.Common;

public sealed class JsonNetResultOverrideAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.Result is JsonResult jsonResult)
        {
            filterContext.Result = jsonResult.ToJsonNetResult();
        }

        base.OnActionExecuted(filterContext);
    }
}

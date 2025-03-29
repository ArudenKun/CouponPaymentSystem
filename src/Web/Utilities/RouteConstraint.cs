using System.Web;
using System.Web.Routing;

namespace Web.Utilities;

public class RouteConstraint<T> : IRouteConstraint
{
    public bool Match(
        HttpContextBase httpContext,
        Route route,
        string parameterName,
        RouteValueDictionary values,
        RouteDirection routeDirection
    )
    {
        var methodNames = typeof(T).GetMethods().Select(m => m.Name.ToLower());
        return methodNames.Contains(values["action"].ToString().ToLower());
    }
}

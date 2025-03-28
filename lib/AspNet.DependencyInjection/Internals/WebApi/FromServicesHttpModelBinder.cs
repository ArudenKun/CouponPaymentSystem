using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace AspNet.DependencyInjection.Internals.WebApi;

internal sealed class FromServicesHttpModelBinder : IModelBinder
{
    public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
    {
        var dependencyScope = actionContext.Request.GetDependencyScope();
        var service = dependencyScope.GetService(bindingContext.ModelType);
        bindingContext.Model = service;
        return service != null;
    }
}

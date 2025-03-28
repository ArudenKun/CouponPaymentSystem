using System.Web.Mvc;

namespace AspNet.DependencyInjection.Internals.Mvc;

internal sealed class FromServicesMvcModelBinder : IModelBinder
{
    public object BindModel(
        ControllerContext controllerContext,
        ModelBindingContext bindingContext
    ) => DependencyResolver.Current.GetService(bindingContext.ModelType);
}

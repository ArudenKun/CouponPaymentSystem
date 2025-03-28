using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.DependencyInjection.Internals;

internal class ScopedMvcDependencyResolver : IDependencyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ScopedMvcDependencyResolver(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public object GetService(Type serviceType) =>
        GetServiceScope().ServiceProvider.GetService(serviceType);

    public IEnumerable<object?> GetServices(Type serviceType) =>
        GetServiceScope().ServiceProvider.GetServices(serviceType);

    private IServiceScope GetServiceScope()
    {
        if (HttpContext.Current.Items[ScopedMvcHttpModule.Key] is IServiceScope scope)
            return scope;

        scope = _serviceProvider.CreateScope();
        HttpContext.Current.Items[ScopedMvcHttpModule.Key] = scope;
        return scope;
    }
}

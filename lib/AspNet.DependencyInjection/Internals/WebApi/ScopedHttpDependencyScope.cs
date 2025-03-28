using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.DependencyInjection.Internals.WebApi;

internal class ScopedHttpDependencyScope : IDependencyScope
{
    private readonly IServiceScope _serviceScope;

    public ScopedHttpDependencyScope(IServiceScope serviceScope)
    {
        _serviceScope = serviceScope;
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }

    public object GetService(Type serviceType)
    {
        return _serviceScope.ServiceProvider.GetService(serviceType);
    }

    public IEnumerable<object?> GetServices(Type serviceType)
    {
        return _serviceScope.ServiceProvider.GetServices(serviceType);
    }
}

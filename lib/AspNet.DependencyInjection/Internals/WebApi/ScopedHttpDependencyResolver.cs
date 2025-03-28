using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.DependencyInjection.Internals.WebApi;

internal sealed class ScopedHttpDependencyResolver : ScopedHttpDependencyScope, IDependencyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ScopedHttpDependencyResolver(IServiceProvider serviceProvider)
        : base(serviceProvider.CreateScope())
    {
        _serviceProvider = serviceProvider;
    }

    public IDependencyScope BeginScope()
    {
        return new ScopedHttpDependencyScope(_serviceProvider.CreateScope());
    }
}

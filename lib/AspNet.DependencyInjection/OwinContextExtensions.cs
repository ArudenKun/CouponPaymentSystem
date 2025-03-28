using AspNet.DependencyInjection.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;

namespace AspNet.DependencyInjection;

public static class OwinContextExtensions
{
    public static IServiceScope? GetServiceScope(this IOwinContext context)
    {
        context.Environment.TryGetValue(
            ScopedMvcDependencyMiddleware.PerRequestServiceScopeKey,
            out var scope
        );
        return scope as IServiceScope;
    }
}

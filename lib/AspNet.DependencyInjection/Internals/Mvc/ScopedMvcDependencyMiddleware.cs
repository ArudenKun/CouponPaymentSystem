using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;

namespace AspNet.DependencyInjection.Internals.Mvc;

internal class ScopedMvcDependencyMiddleware : OwinMiddleware
{
    private readonly IServiceProvider _serviceProvider;

    internal static readonly string PerRequestServiceScopeKey = GuidPolyfill
        .CreateVersion7()
        .ToString();

    public ScopedMvcDependencyMiddleware(OwinMiddleware next, IServiceProvider serviceProvider)
        : base(next)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task Invoke(IOwinContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        SetDependencyScope(context, scope);
        return Next.Invoke(context);
    }

    private static void SetDependencyScope(IOwinContext context, IServiceScope scope)
    {
        if (context.GetServiceScope() is null)
        {
            context.Environment.Add(PerRequestServiceScopeKey, scope);
        }
    }
}

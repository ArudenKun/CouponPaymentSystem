using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace Castle.Facilities.AspNet.SystemWeb;

public class WebRequestScopeAccessor : IScopeAccessor
{
    public void Dispose()
    {
        var scope = PerWebRequestLifestyleModule.DetachScope();
        scope?.Dispose();
    }

    public ILifetimeScope? GetScope(CreationContext context)
    {
        return PerWebRequestLifestyleModule.AttachScope();
    }
}

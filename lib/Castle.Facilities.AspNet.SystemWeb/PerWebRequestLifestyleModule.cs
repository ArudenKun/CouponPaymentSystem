using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace Castle.Facilities.AspNet.SystemWeb;

public class PerWebRequestLifestyleModule : IHttpModule
{
    private const string Key = "castle.per-web-request-lifestyle-cache";

    private static bool _initialized;

    public void Init(HttpApplication context)
    {
        _initialized = true;
        context.EndRequest += EndRequest;
    }

    protected void EndRequest(Object sender, EventArgs e)
    {
        var scope = GetOrCreateScope(createIfNotPresent: false);
        scope?.Dispose();
    }

    internal static ILifetimeScope? AttachScope()
    {
        EnsureInitialized();

        var context = HttpContext.Current;
        if (context == null)
        {
            throw new InvalidOperationException(
                "HttpContext.Current is null. PerWebRequestLifestyle can only be used in ASP.Net"
            );
        }

        return GetOrCreateScope(createIfNotPresent: true);
    }

    internal static ILifetimeScope? DetachScope()
    {
        var scope = GetOrCreateScope(createIfNotPresent: true);
        if (scope != null)
        {
            HttpContext.Current.Items.Remove(Key);
        }

        return scope;
    }

    private static void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        throw new ComponentResolutionException(
            $"PerWebRequestLifestyleModule was not initialised. Please refer to https://github.com/castleproject/Windsor/blob/master/docs/systemweb-facility.md for more info."
        );
    }

    private static ILifetimeScope? GetOrCreateScope(bool createIfNotPresent)
    {
        var context = HttpContext.Current;
        if (context == null)
        {
            return null;
        }

        var candidates = (ILifetimeScope)context.Items[Key];
        if (candidates == null && createIfNotPresent)
        {
            candidates = new DefaultLifetimeScope(new ScopeCache());
            context.Items[Key] = candidates;
        }

        return candidates;
    }

    public void Dispose() { }
}

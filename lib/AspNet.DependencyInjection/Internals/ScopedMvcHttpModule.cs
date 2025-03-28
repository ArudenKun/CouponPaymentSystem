using System.Web;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.DependencyInjection.Internals;

internal sealed class ScopedMvcHttpModule : IHttpModule
{
    internal static readonly string Key = GuidPolyfill.CreateVersion7().ToString();

    public void Init(HttpApplication context)
    {
        Guard.NotNull(context);
        context.EndRequest += ContextOnEndRequest;
    }

    private void ContextOnEndRequest(object sender, EventArgs e)
    {
        var app = (HttpApplication)sender;
        var scope = app.Context.Items[Key] as IServiceScope;
        scope?.Dispose();
    }

    public void Dispose() { }
}

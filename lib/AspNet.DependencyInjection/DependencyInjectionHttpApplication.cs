using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using DryIoc.Mvc;
using DryIoc.Owin;
using Microsoft.Extensions.DependencyInjection;
using Owin;

namespace AspNet.DependencyInjection;

public abstract class DependencyInjectionHttpApplication : HttpApplication
{
    private IContainer? _container;

    protected abstract Assembly Assembly { get; }

    // ReSharper disable once UnusedMember.Global
    public void Configuration(IAppBuilder app)
    {
        BuildServiceProvider();
        Guard.NotNull(_container);

        app.UseDryIocOwinMiddleware(_container);
        Configure(app, _container);
    }

    protected abstract void Configure(IAppBuilder app, IServiceProvider serviceProvider);

    protected abstract void ConfigureServices(IServiceCollection services);

    protected virtual void ConfigureRoutes(RouteCollection routes) { }

    protected virtual void ConfigureFilters(GlobalFilterCollection filters) { }

    protected virtual void ConfigureBundles(BundleCollection bundles) { }

    protected void Application_Start()
    {
        ConfigureRoutes(RouteTable.Routes);
        ConfigureFilters(GlobalFilters.Filters);
        ConfigureBundles(BundleTable.Bundles);
        OnApplicationStart();
    }

    protected virtual void OnApplicationStart() { }

    protected void Application_End()
    {
        OnApplicationEnd();
        _container?.Dispose();
        _container = null;
    }

    protected virtual void OnApplicationEnd() { }

    private void BuildServiceProvider()
    {
        if (_container is not null)
        {
            return;
        }

        var container = new Container();
        var services = new ServiceCollection();
        ConfigureServices(services);
        _container = container.WithDependencyInjectionAdapter(services);
        _container.WithMvc();
    }
}

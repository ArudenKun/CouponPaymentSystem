using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet.DependencyInjection;
using AspNet.DependencyInjection.Internals;
using Microsoft.Extensions.DependencyInjection;
using Owin;

[assembly: PreApplicationStartMethod(
    typeof(DependencyInjectionHttpApplication),
    nameof(DependencyInjectionHttpApplication.InitModule)
)]

namespace AspNet.DependencyInjection;

public abstract class DependencyInjectionHttpApplication : HttpApplication
{
    private static ServiceProvider? _serviceProvider;

    public static void InitModule() => RegisterModule(typeof(ScopedMvcHttpModule));

    protected abstract Assembly Assembly { get; }

    // ReSharper disable once UnusedMember.Global
    public void Configuration(IAppBuilder app)
    {
        BuildServiceProvider();

        Guard.NotNull(_serviceProvider);
        app.Use<ScopedMvcDependencyMiddleware>(_serviceProvider);
        DependencyResolver.SetResolver(new ScopedMvcDependencyResolver(_serviceProvider));
        Configure(app, _serviceProvider);
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
        _serviceProvider?.Dispose();
        _serviceProvider = null;
    }

    protected virtual void OnApplicationEnd() { }

    private void BuildServiceProvider()
    {
        if (_serviceProvider is not null)
        {
            return;
        }

        var services = new ServiceCollection();
        services.AddMvc(Assembly);
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider(true);
    }
}

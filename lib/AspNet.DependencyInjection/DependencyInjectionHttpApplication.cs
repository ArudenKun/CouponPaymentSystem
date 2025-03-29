using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Owin;

namespace AspNet.DependencyInjection;

public abstract class DependencyInjectionHttpApplication : HttpApplication
{
    private static IContainer? _container;
    private static AutofacServiceProvider? _serviceProvider;

    protected abstract Assembly Assembly { get; }

    // ReSharper disable once UnusedMember.Global
    public void Configuration(IAppBuilder app)
    {
        BuildServiceProvider();
        Guard.NotNull(_container);
        Guard.NotNull(_serviceProvider);

        app.UseAutofacMiddleware(_container);
        DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

        app.UseAutofacMiddleware(_container);
        app.UseAutofacMvc();

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
        _container?.Dispose();
        _serviceProvider?.Dispose();
        _container = null;
        _serviceProvider = null;
    }

    protected virtual void OnApplicationEnd() { }

    private void BuildServiceProvider()
    {
        if (_container is not null)
        {
            return;
        }

        var builder = new ContainerBuilder();

        builder.RegisterControllers(Assembly);
        builder.RegisterModelBinders(Assembly);
        builder.RegisterModelBinderProvider();
        builder.RegisterModule<AutofacWebTypesModule>();
        builder.RegisterSource(new ViewRegistrationSource());
        builder.RegisterFilterProvider();

        var services = new ServiceCollection();
        ConfigureServices(services);
        builder.Populate(services);
        _container = builder.Build();
        _serviceProvider = new AutofacServiceProvider(_container);
    }
}

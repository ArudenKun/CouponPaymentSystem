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
        BuildServices();

        Guard.NotNull(_container);
        Guard.NotNull(_serviceProvider);

        // Owin
        app.UseAutofacMiddleware(_container);

        // Owin MVC
        app.UseAutofacMvc();

        DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

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

    private void BuildServices()
    {
        if (_container is not null && _serviceProvider is not null)
        {
            return;
        }

        var containerBuilder = new ContainerBuilder();

        // MVC
        containerBuilder.RegisterControllers(Assembly).InstancePerRequest();
        containerBuilder.RegisterModelBinders(Assembly).InstancePerRequest();
        containerBuilder.RegisterModelBinderProvider();
        containerBuilder.RegisterModule<AutofacWebTypesModule>();
        containerBuilder.RegisterSource(new ViewRegistrationSource());
        containerBuilder.RegisterFilterProvider();

        var services = new ServiceCollection();
        ConfigureServices(services);
        containerBuilder.Populate(services);
        _container = containerBuilder.Build();
        _serviceProvider = new AutofacServiceProvider(_container);
    }
}

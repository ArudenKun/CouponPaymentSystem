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

    protected abstract Assembly Assembly { get; }

    // ReSharper disable once UnusedMember.Global
    public void Configuration(IAppBuilder app)
    {
        BuildServiceProvider();
        Guard.NotNull(_container);

        app.UseAutofacMiddleware(_container);
        app.UseAutofacMvc();
        Configure(app, new AutofacServiceProvider(_container));
    }

    protected abstract void Configure(IAppBuilder app, IServiceProvider serviceProvider);

    protected abstract void ConfigureServices(IServiceCollection services);

    protected virtual void ConfigureFilters(GlobalFilterCollection filters) { }

    protected virtual void ConfigureRoutes(RouteCollection routes) { }

    protected virtual void ConfigureBundles(BundleCollection bundles) { }

    protected void Application_Start()
    {
        ConfigureFilters(GlobalFilters.Filters);
        ConfigureRoutes(RouteTable.Routes);
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

        var container = new ContainerBuilder();

        // Register your MVC controllers. (MvcApplication is the name of
        // the class in Global.asax.)
        container.RegisterControllers(Assembly);

        // OPTIONAL: Register model binders that require DI.
        container.RegisterModelBinders(Assembly);
        container.RegisterModelBinderProvider();

        // OPTIONAL: Register web abstractions like HttpContextBase.
        container.RegisterModule<AutofacWebTypesModule>();

        // OPTIONAL: Enable property injection in view pages.
        container.RegisterSource(new ViewRegistrationSource());

        // OPTIONAL: Enable property injection into action filters.
        container.RegisterFilterProvider();

        var services = new ServiceCollection();
        ConfigureServices(services);
        container.Populate(services);
        _container = container.Build();
        DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
    }
}

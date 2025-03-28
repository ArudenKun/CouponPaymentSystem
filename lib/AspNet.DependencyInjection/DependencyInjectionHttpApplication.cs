using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Owin;

namespace AspNet.DependencyInjection;

public abstract class DependencyInjectionHttpApplication : HttpApplication
{
    private static IContainer? _container;
    private static AutofacServiceProvider? _serviceProvider;

    private static HttpConfiguration HttpConfiguration => GlobalConfiguration.Configuration;

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

        // Owin Web Api
        app.UseAutofacWebApi(HttpConfiguration);
        app.UseWebApi(HttpConfiguration);

        Configure(app, _serviceProvider);
    }

    protected abstract void Configure(IAppBuilder app, IServiceProvider serviceProvider);

    protected abstract void ConfigureServices(IServiceCollection services);

    protected void Application_Start() => OnApplicationStart();

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
        if (_container is not null)
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

        // Web Api
        containerBuilder.RegisterApiControllers(Assembly).InstancePerRequest();
        containerBuilder.RegisterWebApiFilterProvider(HttpConfiguration);
        containerBuilder.RegisterWebApiModelBinderProvider();

        var services = new ServiceCollection();
        ConfigureServices(services);
        containerBuilder.Populate(services);
        _container = containerBuilder.Build();
        _serviceProvider = new AutofacServiceProvider(_container);

        DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        HttpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
    }
}

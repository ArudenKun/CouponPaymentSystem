using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AspNet.DependencyInjection;
using AspNet.DependencyInjection.Internals.Mvc;
using AspNet.DependencyInjection.Internals.WebApi;
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

    // ReSharper disable once UnusedMember.Global
    public void Configuration(IAppBuilder app)
    {
        BuildServiceProvider();

        Guard.NotNull(_serviceProvider);
        app.Use<ScopedMvcDependencyMiddleware>(_serviceProvider);
        DependencyResolver.SetResolver(new ScopedMvcDependencyResolver(_serviceProvider));
        GlobalConfiguration.Configure(configuration =>
            configuration.DependencyResolver = new ScopedHttpDependencyResolver(_serviceProvider)
        );

        Configure(app, _serviceProvider);
    }

    protected abstract void Configure(IAppBuilder app, IServiceProvider serviceProvider);

    protected abstract void ConfigureServices(IServiceCollection services);

    protected void Application_Start() => OnApplicationStart();

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
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider(true);
    }
}

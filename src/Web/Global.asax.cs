using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using Web;
using Web.Controllers;

[assembly: OwinStartup(typeof(MvcApplication))]

namespace Web;

public class MvcApplication : DependencyInjectionHttpApplication
{
    protected override Assembly Assembly => typeof(MvcApplication).Assembly;

    protected override void Configure(IAppBuilder app, IServiceProvider serviceProvider)
    {
        var registered = serviceProvider.GetAutofacRoot().IsRegistered<HomeController>();

        Console.WriteLine($"Home Controller: {registered}");

        // app.UseCookieAuthentication(
        //     new CookieAuthenticationOptions
        //     {
        //         AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
        //         LoginPath = new PathString("/api/auth/login".ToLower()),
        //     }
        // );
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    protected override void OnApplicationStart()
    {
        GlobalConfiguration.Configure(WebApiConfig.Register);
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
}

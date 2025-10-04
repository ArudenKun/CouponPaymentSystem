using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.WebApi;
using Abp.WebApi.Configuration;
using Castle.MicroKernel.Registration;
using Infrastructure;
using Microsoft.Owin.Security;
using Web.Json;
using Web.ModelBinding;

namespace Web;

[DependsOn(
    typeof(InfrastructureModule),
    typeof(AbpWebApiModule),
    typeof(AbpWebSignalRModule),
    typeof(AbpWebMvcModule),
    typeof(AbpWebSignalRModule)
)]
public sealed class WebModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        IocManager.IocContainer.Register(
            Component
                .For<IAuthenticationManager>()
                .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                .LifestyleTransient()
        );

        AreaRegistration.RegisterAllAreas();
        RegisterFilters(GlobalFilters.Filters);
        RegisterRoutes(RouteTable.Routes);
        RegisterBundles(BundleTable.Bundles);
    }

    public override void PostInitialize()
    {
        ModelBinders.Binders.Add(typeof(int), IocManager.Resolve<CloakMvcModelBinder>());
        var httpConfiguration = IocManager.Resolve<IAbpWebApiConfiguration>().HttpConfiguration;
        httpConfiguration.BindParameter(typeof(int), IocManager.Resolve<CloakApiModelBinder>());
        httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
            IocManager.Resolve<CpsWebContractResolver>();
    }

    private static void RegisterFilters(GlobalFilterCollection filters) { }

    private static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
        routes.MapMvcAttributeRoutes();
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
            }
        );
    }

    private static void RegisterBundles(BundleCollection bundles) { }
}

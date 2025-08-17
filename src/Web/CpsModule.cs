using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Owin;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.WebApi;
using Castle.Facilities.AspNet.SystemWeb;
using Castle.MicroKernel.Registration;
using CouponPaymentSystem.Application;
using CouponPaymentSystem.Application.Common.Extensions;
using CouponPaymentSystem.Infrastructure;
using Microsoft.Owin.Security;

namespace CouponPaymentSystem;

[DependsOn(
    typeof(CpsInfrastructureModule),
    typeof(AbpOwinModule),
    typeof(AbpWebMvcModule),
    typeof(AbpWebApiModule),
    typeof(AbpWebSignalRModule),
    typeof(SystemWebModule)
)]
public class CpsModule : AbpModule
{
    private static readonly Assembly ThisAssembly = typeof(CpsModule).Assembly;

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(ThisAssembly);
        IocManager.Register(
            Component
                .For<IAuthenticationManager>()
                .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                .LifestylePerWebRequest()
        );
        Configuration
            .Modules.AbpWebApi()
            .DynamicApiControllerBuilder.ForAll<IApplicationService>(
                typeof(CpsApplicationModule).Assembly,
                "default"
            );
        AreaRegistration.RegisterAllAreas();
        ConfigureRoutes(RouteTable.Routes);
        ConfigureBundles(BundleTable.Bundles);
    }

    private static void ConfigureRoutes(RouteCollection routes)
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

    private static void ConfigureBundles(BundleCollection bundles)
    {
        // Needed
    }
}

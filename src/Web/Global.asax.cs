using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet.DependencyInjection;
using BundleTransformer.Core.Bundles;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Web;
using Web.Controllers.Common;
using Web.Utilities;
using Web.Utilities.Extensions;

[assembly: OwinStartup(typeof(MvcApplication))]

namespace Web;

public class MvcApplication : DependencyInjectionHttpApplication
{
    protected override Assembly Assembly => typeof(MvcApplication).Assembly;

    protected override void Configure(IAppBuilder app, IServiceProvider serviceProvider)
    {
        app.UseCookieAuthentication(
            new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/auth/login"),
                CookieName = CookieAuthenticationDefaults.CookiePrefix + "CouponPaymentSystem.",
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                ReturnUrlParameter = "returnUrl",
                CookieSecure = CookieSecureOption.SameAsRequest,
                SlidingExpiration = true,
            }
        );
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    protected override void ConfigureFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
        filters.Add(new JsonNetResultOverrideAttribute());
    }

    protected override void ConfigureRoutes(RouteCollection routes)
    {
        routes.LowercaseUrls = true;
        routes.AppendTrailingSlash = true;
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapMvcAttributeRoutes();

        routes.MapRoute(
            name: "Home",
            url: "{action}/{id}",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
            }
        );

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

    protected override void ConfigureBundles(BundleCollection bundles)
    {
        bundles.Add(
            new CustomStyleBundle("~/bundles/css/base")
                .NullOrderer()
                .Include("~/Lib/bootstrap/css/bootstrap.min.css")
                .Include("~/Lib/bootstrap-icons/font/bootstrap-icons.min.css")
                .Include("~/Content/css/style.css")
        );

        bundles.Add(
            new CustomStyleBundle("~/bundles/css/datatables")
                .NullOrderer()
                .Include("~/Lib/datatables/datatables.min.css")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/base/preload")
                .NullOrderer()
                .Include("~/Lib/jquery/jquery.min.js")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/base")
                .NullOrderer()
                .Include("~/Lib/jquery/jquery.unobtrusive-ajax.min.js")
                .Include("~/Lib/bootstrap/js/bootstrap.bundle.min.js")
                .Include("~/Lib/sweetalert2/sweetalert2.all.min.js")
                .Include("~/Scripts/script.js")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/jqueryval")
                .NullOrderer()
                .IncludeDirectory("~/Lib/jquery-validate", "*.js")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/datatables")
                .NullOrderer()
                .Include("~/Lib/datatables/datatables.min.js")
        );

        BundleTable.EnableOptimizations = !Helper.IsDebug;
    }
}

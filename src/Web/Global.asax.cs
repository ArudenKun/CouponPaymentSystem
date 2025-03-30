using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNet.DependencyInjection;
using DataTables.AspNet.Mvc5;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using Web;

[assembly: OwinStartup(typeof(MvcApplication))]

namespace Web;

public class MvcApplication : DependencyInjectionHttpApplication
{
    protected override void Configure(IAppBuilder app, IServiceProvider serviceProvider)
    {
        DataTableConfiguration.RegisterDataTables();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // services.AddApplication().AddInfrastructure(Helper.IsDebug).AddWeb();
    }

    protected override void ConfigureFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
        // filters.Add(new JsonNetResultOverrideAttribute());
    }

    protected override void ConfigureRoutes(RouteCollection routes)
    {
        routes.LowercaseUrls = true;
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
        // bundles.Add(
        //     new CustomStyleBundle("~/bundles/css/base")
        //         //.NullOrderer()
        //         .Include("~/Lib/bootstrap/css/bootstrap.min.css")
        //         .Include("~/Lib/bootstrap-icons/font/bootstrap-icons.min.css")
        //         .Include("~/Content/css/style.css")
        // );
        //
        // bundles.Add(
        //     new CustomStyleBundle("~/bundles/css/datatables")
        //     //.NullOrderer()
        //     .Include("~/Lib/datatables/datatables.min.css")
        // );
        //
        // bundles.Add(
        //     new CustomScriptBundle("~/bundles/js/base/preload")
        //     //.NullOrderer()
        //     .Include("~/Lib/jquery/jquery.min.js")
        // );
        //
        // bundles.Add(
        //     new CustomScriptBundle("~/bundles/js/base")
        //         //.NullOrderer()
        //         .Include("~/Lib/jquery/jquery.unobtrusive-ajax.min.js")
        //         .Include("~/Lib/bootstrap/js/bootstrap.bundle.min.js")
        //         .Include("~/Lib/sweetalert2/sweetalert2.all.min.js")
        //         .Include("~/Scripts/script.js")
        // );
        //
        // bundles.Add(
        //     new CustomScriptBundle("~/bundles/js/jqueryval")
        //     //.NullOrderer()
        //     .IncludeDirectory("~/Lib/jquery-validate", "*.js")
        // );
        //
        // bundles.Add(
        //     new CustomScriptBundle("~/bundles/js/datatables")
        //     // .NullOrderer()
        //     .Include("~/Lib/datatables/datatables.min.js")
        // );

        // BundleTable.EnableOptimizations = !Helper.IsDebug;
    }
}

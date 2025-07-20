using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Application.Services;
using Abp.BlobStoring;
using Abp.BlobStoring.FileSystem;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Owin;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.WebApi;
using Castle.MicroKernel.Registration;
using CouponPaymentSystem.Application;
using CouponPaymentSystem.Core.Common;
using CouponPaymentSystem.Core.Common.Extensions;
using CouponPaymentSystem.Core.Configuration.Options;
using Hangfire;
using Hangfire.SqlServer;
using Medallion.Threading;
using Medallion.Threading.SystemSqlServer;
using Microsoft.Owin.Security;

namespace CouponPaymentSystem;

[DependsOn(
    typeof(CpsApplicationModule),
    typeof(AbpOwinModule),
    typeof(AbpWebApiModule),
    typeof(AbpWebSignalRModule),
    typeof(AbpHangfireModule),
    typeof(AbpWebMvcModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(SystemWebModule)
)]
public class CpsModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.BackgroundJobs.UseHangfire(c =>
        {
            var options = new SqlServerStorageOptions
            {
                SqlClientFactory = SqlClientFactory.Instance,
                PrepareSchemaIfNecessary = false,
            };
            c.GlobalConfiguration.UseSqlServerStorage(
                Configuration.DefaultNameOrConnectionString,
                options
            );
        });
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(CpsModule).Assembly;
        IocManager.RegisterAssemblyByConvention(thisAssembly);
        IocManager.Register(
            Component
                .For<IAuthenticationManager>()
                .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                .LifestylePerWebRequest(),
            Component
                .For<
                    IDistributedLockProvider,
                    IDistributedUpgradeableReaderWriterLock,
                    IDistributedSemaphore
                >()
                .UsingFactoryMethod(kernel => new SqlDistributedSynchronizationProvider(
                    kernel.Resolve<CpsOptions>().Cps.ConnectionString
                ))
                .LifestyleSingleton()
        );

        Configuration
            .Modules.AbpBlobStoring()
            .Containers.Configure<AbpBlobStoringOptions>(options =>
            {
                options.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = IocManager.Resolve<IPathManager>().BlobsDir;
                });
            });
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

    public override void Shutdown()
    {
        IocManager.Resolve<CpsOptions>().Save();
    }

    private void ConfigureRoutes(RouteCollection routes)
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

    private void ConfigureBundles(BundleCollection bundles) { }
}

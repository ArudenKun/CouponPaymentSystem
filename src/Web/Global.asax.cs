using System.Web.Hosting;
using System.Web.Mvc;
using Abp.Configuration;
using Abp.Owin;
using Abp.Web;
using Hangfire;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Web;
using Web.Extensions;

[assembly: OwinStartup(typeof(MvcApplication), nameof(MvcApplication.Configuration))]

namespace Web;

public class MvcApplication : AbpWebApplication<WebModule>
{
    public void Configuration(IAppBuilder app)
    {
        app.UseAbp();
        app.UseHttpOnlyAntiforgeryToken();
        app.UseCookieAuthentication(new CookieAuthenticationOptions { });
        app.UseHangfireAspNet(() =>
            [AbpBootstrapper.IocManager.Resolve<IAbpHangfireConfiguration>().Server]
        );
        app.UseHangfireDashboard(
            "/jobs-dashboard",
            new DashboardOptions
            {
                AppPath = HostingEnvironment.ApplicationVirtualPath,
                Authorization = [],
            }
        );
        app.MapSignalR();
    }

    protected override void Application_Start(object sender, EventArgs e)
    {
        MvcHandler.DisableMvcResponseHeader = true;
        base.Application_Start(sender, e);
    }
}

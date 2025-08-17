using Abp.Castle.Logging.Log4Net;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Owin;
using Abp.Web;
using Castle.Facilities.Logging;
using CouponPaymentSystem;
using Hangfire;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(MvcApplication))]

namespace CouponPaymentSystem;

public class MvcApplication : AbpWebApplication<CpsModule>
{
    [UsedImplicitly]
    public void Configuration(IAppBuilder app)
    {
        app.UseAbp();
        app.UseCookieAuthentication(
            new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                CookieName = CookieAuthenticationDefaults.CookiePrefix + ".CPS.",
                LoginPath = new PathString("/authentication/sign-in"),
                ReturnUrlParameter = "returnUrl",
                ExpireTimeSpan = 15.Minutes(),
                SlidingExpiration = true,
                CookieHttpOnly = true,
                CookieSameSite = SameSiteMode.Lax,
                CookieSecure = CookieSecureOption.Always,
            }
        );
        app.UseHangfireAspNet(() =>
            [AbpBootstrapper.IocManager.Resolve<IAbpHangfireConfiguration>().Server]
        );
        app.UseHangfireDashboard(
            "/hangfire",
            new DashboardOptions
            {
                Authorization = [new AbpHangfireAuthorizationFilter()],
                AsyncAuthorization = [new AbpHangfireAsyncAuthorizationFilter()],
            }
        );
        app.MapSignalR();
    }

    protected override void Application_Start(object sender, EventArgs e)
    {
#if DEBUG
        AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f =>
            f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.dev.config"))
        );
#else
        AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f =>
            f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
        );
#endif

        base.Application_Start(sender, e);
    }
}

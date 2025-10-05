using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Web.Extensions;

public static class OwinExtensions
{
    public static IAppBuilder UseHttpOnlyAntiforgeryToken(this IAppBuilder app) =>
        app.Use(
            async (ctx, next) =>
            {
                var token = ctx.Request.Cookies["XSRF-TOKEN"];
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrWhiteSpace(token))
                    ctx.Request.Headers.Add("XSRF-TOKEN", [token]);
                await next();
            }
        );

    public static IAppBuilder UseAspNetCoreStaticFiles(this IAppBuilder app) =>
        app.UseStaticFiles(
            new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem("wwwroot"),
                RequestPath = PathString.Empty,
            }
        );
}

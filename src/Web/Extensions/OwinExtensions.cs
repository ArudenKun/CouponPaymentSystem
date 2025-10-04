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
}

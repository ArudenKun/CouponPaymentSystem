using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private static Assembly? _assembly;

    public static IServiceCollection AddMvc(
        this IServiceCollection services,
        Assembly? assembly = null
    )
    {
        _assembly = assembly ?? Assembly.GetCallingAssembly();
        foreach (
            var controllerType in _assembly
                .GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t =>
                    typeof(IController).IsAssignableFrom(t)
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                )
        )
        {
            services.AddTransient(controllerType);
        }

        // HttpContext Properties
        services.AddTransient<HttpContextBase>(_ => new HttpContextWrapper(HttpContext.Current));
        services.AddTransient(sp => sp.GetRequiredService<HttpContextBase>().Request);
        services.AddTransient(sp => sp.GetRequiredService<HttpContextBase>().Response);
        services.AddTransient(sp => sp.GetRequiredService<HttpContextBase>().Server);
        services.AddTransient(sp => sp.GetRequiredService<HttpContextBase>().Session);
        services.AddTransient(sp => sp.GetRequiredService<HttpContextBase>().Application);

        // HttpRequest Properties
        services.AddTransient(sp => sp.GetRequiredService<HttpRequestBase>().Browser);
        services.AddTransient(sp => sp.GetRequiredService<HttpRequestBase>().Files);
        services.AddTransient(sp => sp.GetRequiredService<HttpRequestBase>().RequestContext);

        // HttpResponse Properties
        services.AddTransient(sp => sp.GetRequiredService<HttpResponseBase>().Cache);

        // HostingEnvironment Properties
        services.AddTransient(_ => HostingEnvironment.VirtualPathProvider);

        // Mvc Types
        services.AddTransient(sp => new UrlHelper(sp.GetRequiredService<RequestContext>()));

        return services;
    }

    public static IServiceCollection AddWebApi(
        this IServiceCollection services,
        Assembly? assembly = null
    )
    {
        _assembly = assembly ?? Assembly.GetCallingAssembly();

        foreach (
            var apiControllerType in _assembly
                .GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t =>
                    typeof(IHttpController).IsAssignableFrom(t)
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                )
        )
        {
            services.AddTransient(apiControllerType);
        }

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Cloaked;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to configure Cloak services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Cloak services to the service collection using a fluent builder pattern.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>A builder for configuring Cloak services.</returns>
    public static ICloakBuilder AddCloak(this IServiceCollection services)
    {
        var builder = new CloakBuilder(services);
        services.AddSingleton(builder.Build);
        return builder;
    }
}

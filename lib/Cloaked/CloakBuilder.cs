using Microsoft.Extensions.DependencyInjection;

namespace Cloaked;

/// <summary>
/// A builder for configuring Cloak services.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CloakBuilder"/> class.
/// </remarks>
public class CloakBuilder : ICloakBuilder
{
    /// <summary>
    /// A builder for configuring Cloak services.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CloakBuilder"/> class.
    /// </remarks>
    /// <param name="services">The service collection to add services to.</param>
    public CloakBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// Gets the service collection that Cloak services are being added to.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets a value indicating whether a codec has already been configured for this builder.
    /// </summary>
    public bool HasCodecConfigured { get; private set; }

    /// <summary>
    /// Marks the builder as having a codec configured.
    /// </summary>
    public void MarkCloakConfigured()
    {
        HasCodecConfigured = true;
    }

    /// <summary>
    /// Builds the Cloak services and returns the configured CloakContractResolver.
    /// </summary>
    /// <param name="serviceProvider">The service provider to build from.</param>
    /// <returns>The configured CloakContractResolver.</returns>
    public CloakTypeInfoResolver Build(IServiceProvider serviceProvider)
    {
        var codec = serviceProvider.GetRequiredService<ICloak>();
        return new CloakTypeInfoResolver(codec);
    }
}

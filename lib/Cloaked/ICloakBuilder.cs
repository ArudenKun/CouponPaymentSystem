using Microsoft.Extensions.DependencyInjection;

namespace Cloaked;

/// <summary>
/// A builder interface for configuring Cloak services.
/// </summary>
public interface ICloakBuilder
{
    /// <summary>
    /// Gets the service collection that Cloak services are being added to.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Gets a value indicating whether a codec has already been configured for this builder.
    /// </summary>
    bool HasCodecConfigured { get; }

    /// <summary>
    /// Marks the builder as having a codec configured.
    /// </summary>
    void MarkCloakConfigured();

    // /// <summary>
    // /// Builds the Cloak services and returns the configured service provider factory.
    // /// </summary>
    // /// <param name="serviceProvider">The service provider to build from.</param>
    // /// <returns>The configured CloakContractResolver.</returns>
    // CloakContractResolver Build(IServiceProvider serviceProvider);
}

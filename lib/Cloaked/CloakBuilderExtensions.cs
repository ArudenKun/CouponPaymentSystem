using Microsoft.Extensions.DependencyInjection;

namespace Cloaked;

/// <summary>
/// Extension methods for configuring Cloak with custom codecs.
/// </summary>
public static class CloakBuilderExtensions
{
    /// <summary>
    /// Configures Cloak to use a custom codec implementation.
    /// </summary>
    /// <typeparam name="TCodec">The type of the custom codec that implements ICloak.</typeparam>
    /// <param name="builder">The Cloak builder.</param>
    /// <returns>The updated Cloak builder for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a codec has already been configured.</exception>
    /// <remarks>
    /// When multiple ICloak implementations are registered in the service container,
    /// .NET's dependency injection returns the LAST registered service, which can lead to
    /// unpredictable behavior. This validation prevents accidental multiple codec registrations.
    ///
    /// If you need multiple codec support, consider implementing a composite codec pattern
    /// or use separate service scopes for different codec configurations.
    /// </remarks>
    public static ICloakBuilder WithCustomCloak<TCodec>(this ICloakBuilder builder)
        where TCodec : class, ICloak
    {
        if (builder.HasCodecConfigured)
        {
            throw new InvalidOperationException(
                "A codec has already been configured for this Cloak builder. "
                    + "You cannot call WithCustomCodec() after another codec configuration method has been called. "
                    + "Multiple codec registrations can lead to unpredictable behavior since .NET DI returns the last registered service. "
                    + "Use separate service scopes or implement a composite codec pattern if you need multiple codec support."
            );
        }

        // Register the custom codec as the ICloak
        builder.Services.AddSingleton<ICloak, TCodec>();

        // Mark codec as configured
        builder.MarkCloakConfigured();

        return builder;
    }

    /// <summary>
    /// Configures Cloak to use a custom codec instance.
    /// </summary>
    /// <param name="builder">The Cloak builder.</param>
    /// <param name="codecInstance">The custom codec instance that implements ICloak.</param>
    /// <returns>The updated Cloak builder for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a codec has already been configured.</exception>
    /// <remarks>
    /// When multiple ICloak implementations are registered in the service container,
    /// .NET's dependency injection returns the LAST registered service, which can lead to
    /// unpredictable behavior. This validation prevents accidental multiple codec registrations.
    /// </remarks>
    public static ICloakBuilder WithCustomCloak(this ICloakBuilder builder, ICloak codecInstance)
    {
        if (builder.HasCodecConfigured)
        {
            throw new InvalidOperationException(
                "A codec has already been configured for this Cloak builder. "
                    + "You cannot call WithCustomCodec() after another codec configuration method has been called. "
                    + "Multiple codec registrations can lead to unpredictable behavior since .NET DI returns the last registered service."
            );
        }

        // Register the custom codec instance as the ICloak
        builder.Services.AddSingleton(codecInstance);

        // Mark codec as configured
        builder.MarkCloakConfigured();

        return builder;
    }

    /// <summary>
    /// Configures Cloak to use a custom codec with a factory method.
    /// </summary>
    /// <param name="builder">The Cloak builder.</param>
    /// <param name="codecFactory">A factory method that creates the custom codec instance.</param>
    /// <returns>The updated Cloak builder for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a codec has already been configured.</exception>
    /// <remarks>
    /// When multiple ICloak implementations are registered in the service container,
    /// .NET's dependency injection returns the LAST registered service, which can lead to
    /// unpredictable behavior. This validation prevents accidental multiple codec registrations.
    /// </remarks>
    public static ICloakBuilder WithCustomCloak(
        this ICloakBuilder builder,
        Func<IServiceProvider, ICloak> codecFactory
    )
    {
        if (builder.HasCodecConfigured)
        {
            throw new InvalidOperationException(
                "A codec has already been configured for this Cloak builder. "
                    + "You cannot call WithCustomCodec() after another codec configuration method has been called. "
                    + "Multiple codec registrations can lead to unpredictable behavior since .NET DI returns the last registered service."
            );
        }

        // Register the custom codec factory as the ICloak
        builder.Services.AddSingleton(codecFactory);

        // Mark codec as configured
        builder.MarkCloakConfigured();

        return builder;
    }
}

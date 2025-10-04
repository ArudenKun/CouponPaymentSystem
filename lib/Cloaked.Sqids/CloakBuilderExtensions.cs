using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Cloaked.Sqids;

/// <summary>
/// Extension methods for configuring Cloak with Sqids encoding.
/// </summary>
public static class CloakBuilderExtensions
{
    /// <summary>
    /// Configures Cloak to use Sqids encoding with the specified options.
    /// </summary>
    /// <param name="builder">The Cloak builder.</param>
    /// <param name="configureOptions">Optional action to configure Cloak options.</param>
    /// <returns>The updated Cloak builder for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a codec has already been configured.</exception>
    /// <remarks>
    /// This method prevents multiple codec registrations to avoid unpredictable behavior.
    /// When multiple ICloak implementations are registered, .NET DI returns the last registered service.
    /// </remarks>
    public static ICloakBuilder WithSqids(
        this ICloakBuilder builder,
        Action<CloakOptions>? configureOptions = null
    )
    {
        if (builder.HasCodecConfigured)
        {
            throw new InvalidOperationException(
                "A codec has already been configured for this Cloak builder. "
                    + "You cannot call WithSqids() after another codec configuration method has been called. "
                    + "Multiple codec registrations can lead to unpredictable behavior since .NET DI returns the last registered service."
            );
        }

        // Configure options
        var options = new CloakOptions();
        configureOptions?.Invoke(options);

        // Register Sqids encoders for different numeric types
        builder.Services.AddSingleton(_ =>
        {
            var sqidsOptions = new SqidsOptions { MinLength = options.MinLength };
            if (options.Alphabet != null)
                sqidsOptions.Alphabet = options.Alphabet;
            return new SqidsEncoder(sqidsOptions);
        });

        // Register the SqidsCodec as the ICloak
        builder.Services.AddSingleton<ICloak, SqidsCloak>();

        // Mark codec as configured
        builder.MarkCloakConfigured();

        return builder;
    }

    /// <summary>
    /// Configures Cloak to use a previously registered Sqids setup.
    /// Use this when you already have Sqids encoders registered in your DI container.
    /// </summary>
    /// <param name="builder">The Cloak builder.</param>
    /// <returns>The updated Cloak builder for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a codec has already been configured.</exception>
    /// <remarks>
    /// This method prevents multiple codec registrations to avoid unpredictable behavior.
    /// When multiple ICloak implementations are registered, .NET DI returns the last registered service.
    /// </remarks>
    public static ICloakBuilder WithRegisteredSqids(this ICloakBuilder builder)
    {
        if (builder.HasCodecConfigured)
        {
            throw new InvalidOperationException(
                "A codec has already been configured for this Cloak builder. "
                    + "You cannot call WithRegisteredSqids() after another codec configuration method has been called. "
                    + "Multiple codec registrations can lead to unpredictable behavior since .NET DI returns the last registered service."
            );
        }

        // Just register the SqidsCodec - assumes encoders are already registered
        builder.Services.AddSingleton<ICloak, SqidsCloak>();

        // Mark codec as configured
        builder.MarkCloakConfigured();

        return builder;
    }
}

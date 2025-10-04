using Domain.Configuration;
using Domain.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Domain.Common;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Configures and validates options.
    /// </summary>
    /// <typeparam name="TOptions">The options type that should be added.</typeparam>
    /// <param name="services">The dependency injection container to add options.</param>
    /// <param name="key">
    /// The configuration key that should be used when configuring the options.
    /// If null, the root configuration will be used to configure the options.
    /// </param>
    /// <returns>The dependency injection container.</returns>
    public static IServiceCollection AddCpsOptions<TOptions>(
        this IServiceCollection services,
        string? key = null
    )
        where TOptions : class
    {
        services.AddSingleton<IValidateOptions<TOptions>>(new ValidateCpsOptions<TOptions>(key));
        services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            if (key != null)
            {
                config = config.GetSection(key);
            }

            return new BindOptions<TOptions>(config);
        });

        return services;
    }

    public static void AddConfiguration(this IServiceCollection services)
    {
        services.AddCpsOptions<CpsOptions>();
        services.AddCpsOptions<AsoDatabaseOptions>(nameof(CpsOptions.Aso));
        services.AddCpsOptions<CpsDatabaseOptions>(nameof(CpsOptions.Cps));
    }
}

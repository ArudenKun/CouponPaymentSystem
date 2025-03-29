using System.Reflection;
using Application.Common.Validation;
using Application.Pipeline;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace Application;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        // services.AddValidators();
        services.AddSingleton<AppValidatorFactory>();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                assemblies.Length is 0 ? [Assembly.GetCallingAssembly()] : assemblies
            );
            // config.NotificationPublisher = new ParallelNoWaitPublisher();
            // config.AddRequestPreProcessor(
            //     typeof(IRequestPreProcessor<>),
            //     typeof(ValidationPreProcessor<>)
            // );
            config.AddOpenBehavior(typeof(CacheInvalidationBehaviour<,>));
            config.AddOpenBehavior(typeof(FusionCacheBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            // config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
        });
        return services;
    }

    // [GenerateServiceRegistrations(
    //     AssignableTo = typeof(IValidator<>),
    //     AsSelf = true,
    //     AsImplementedInterfaces = true,
    //     Lifetime = ServiceLifetime.Singleton
    // )]
    // private static partial void AddValidators(this IServiceCollection services);
}

using Application.Common.Validation;
using Application.Pipeline;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace Application;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidators();
        services.AddSingleton<ValidatorFactory>();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            // config.NotificationPublisher = new ParallelNoWaitPublisher();
            // config.AddRequestPreProcessor(
            //     typeof(IRequestPreProcessor<>),
            //     typeof(ValidationPreProcessor<>)
            // );
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(FusionCacheBehaviour<,>));
            config.AddOpenBehavior(typeof(FusionCacheInvalidationBehaviour<,>));
            // config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
        });
        return services;
    }

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IValidator<>),
        AsSelf = true,
        AsImplementedInterfaces = true,
        Lifetime = ServiceLifetime.Transient
    )]
    private static partial void AddValidators(this IServiceCollection services);
}

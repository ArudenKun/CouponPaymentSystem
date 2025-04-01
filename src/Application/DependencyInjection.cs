using Application.Common.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace Application;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ValidatorFactory>();
        services.AddValidators();
        services.AddMediatR();
        return services;
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddTransient<IMediator, Mediator>();
        services.AddMediatRHandlers();
        services.AddMediatRBehaviors();
    }

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IValidator<>),
        AsSelf = true,
        AsImplementedInterfaces = true,
        Lifetime = ServiceLifetime.Transient
    )]
    private static partial void AddValidators(this IServiceCollection services);

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IRequestHandler<,>),
        Lifetime = ServiceLifetime.Transient
    )]
    private static partial void AddMediatRHandlers(this IServiceCollection services);

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IPipelineBehavior<,>),
        Lifetime = ServiceLifetime.Transient
    )]
    private static partial void AddMediatRBehaviors(this IServiceCollection services);
}

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;
using Web.Core.Pipelines;
using ZiggyCreatures.Caching.Fusion;

namespace Web.Core;

public static partial class DependencyInjection
{
    public static void AddCore(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidators();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                assemblies.Length is 0 ? assemblies : [Assembly.GetCallingAssembly()]
            );
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(FusionCacheBehaviour<,>));
            config.AddOpenBehavior(typeof(FusionCacheInvalidationBehaviour<,>));
        });
        services.AddFusionCache().WithDefaultEntryOptions(options => { });
    }

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IValidator<>),
        AsSelf = true,
        AsImplementedInterfaces = true,
        Lifetime = ServiceLifetime.Transient
    )]
    private static partial void AddValidators(this IServiceCollection services);
}

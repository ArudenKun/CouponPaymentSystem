using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CouponPaymentSystem.Application.Common.Extensions;

public static class DependencyExtensions
{
    /// <inheritdoc cref="IWindsorContainer.Register"/>>
    public static IIocManager Register(
        this IIocManager iocManager,
        params IRegistration[] registrations
    )
    {
        iocManager.IocContainer.Register(registrations);
        return iocManager;
    }

    public static IIocManager RegisterWithServiceCollection(
        this IIocManager iocManager,
        Action<IServiceCollection> configure
    )
    {
        iocManager.IocContainer.RegisterWithServiceCollection(configure);
        return iocManager;
    }

    public static IWindsorContainer RegisterWithServiceCollection(
        this IWindsorContainer windsorContainer,
        Action<IServiceCollection> configure
    )
    {
        var services = new ServiceCollection();
        configure(services);
        windsorContainer.AddServices(services);
        return windsorContainer;
    }
}

using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CouponPaymentSystem.Core.Common.Extensions;

public static class WindsorExtensions
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

    public static IIocManager WithServiceCollection(
        this IIocManager iocManager,
        Action<IServiceCollection> registrationAction
    )
    {
        iocManager.IocContainer.WithServiceCollection(registrationAction);
        return iocManager;
    }

    public static IWindsorContainer WithServiceCollection(
        this IWindsorContainer windsorContainer,
        Action<IServiceCollection> registrationAction
    )
    {
        var services = new ServiceCollection();
        registrationAction(services);
        windsorContainer.AddServices(services);
        return windsorContainer;
    }
}

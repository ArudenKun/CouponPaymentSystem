﻿using Microsoft.Extensions.DependencyInjection;

namespace Castle.Windsor.Microsoft.DependencyInjection;

/// <summary>
/// Implements <see cref="IServiceScope"/>.
/// </summary>
public class WindsorServiceScope : IServiceScope
{
    public IServiceProvider ServiceProvider { get; }

    public MsLifetimeScope LifetimeScope { get; }

    private readonly IWindsorContainer _container;
    private readonly IMsLifetimeScope _parentLifetimeScope;

    public WindsorServiceScope(IWindsorContainer container, IMsLifetimeScope currentMsLifetimeScope)
    {
        _container = container;
        _parentLifetimeScope = currentMsLifetimeScope;

        LifetimeScope = new MsLifetimeScope(container);

        _parentLifetimeScope?.AddChild(LifetimeScope);

        using (MsLifetimeScope.Using(LifetimeScope))
        {
            ServiceProvider = container.Resolve<IServiceProvider>();
        }
    }

    public void Dispose()
    {
        _parentLifetimeScope?.RemoveChild(LifetimeScope);
        LifetimeScope.Dispose();
        _container.Release(ServiceProvider);
    }
}

﻿namespace Castle.Windsor.Microsoft.DependencyInjection;

public class GlobalScopedWindsorServiceProvider : ScopedWindsorServiceProvider, IDisposable
{
    public GlobalScopedWindsorServiceProvider(
        IWindsorContainer container,
        MsLifetimeScopeProvider msLifetimeScopeProvider
    )
        : base(container, msLifetimeScopeProvider) { }

    public void Dispose()
    {
        OwnMsLifetimeScope.Dispose();
    }
}

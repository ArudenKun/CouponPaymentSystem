using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;

namespace Castle.Facilities.AspNet.SystemWeb;

public static class LifestyleRegistrationExtensions
{
    public static BasedOnDescriptor LifestylePerWebRequest(this BasedOnDescriptor descriptor)
    {
        return descriptor.Configure(c => c.LifestylePerWebRequest());
    }

    public static ComponentRegistration<TService> LifestylePerWebRequest<TService>(
        this ComponentRegistration<TService> registration
    )
        where TService : class
    {
        return registration.LifeStyle.Scoped<WebRequestScopeAccessor>();
    }

    public static ComponentRegistration<TService> PerWebRequest<TService>(
        this LifestyleGroup<TService> @group
    )
        where TService : class
    {
        return group.Scoped<WebRequestScopeAccessor>();
    }
}

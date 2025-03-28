using System.Web.Http.ModelBinding;
using AspNet.DependencyInjection.Internals.WebApi;

namespace AspNet.DependencyInjection.WebApi;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
public sealed class FromServicesAttribute : ModelBinderAttribute
{
    public FromServicesAttribute()
        : base(typeof(FromServicesHttpModelBinder)) { }
}

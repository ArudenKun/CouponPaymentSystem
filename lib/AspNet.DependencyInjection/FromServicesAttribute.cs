using System.Web.Mvc;
using AspNet.DependencyInjection.Internals;

namespace AspNet.DependencyInjection;

[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Enum
        | AttributeTargets.Interface
        | AttributeTargets.Parameter,
    Inherited = false
)]
public class FromServicesAttribute : CustomModelBinderAttribute
{
    private readonly ModelBinderAttribute _modelBinderAttribute;

    public FromServicesAttribute() =>
        _modelBinderAttribute = new ModelBinderAttribute(typeof(FromServicesMvcModelBinder));

    public override IModelBinder GetBinder() => _modelBinderAttribute.GetBinder();
}

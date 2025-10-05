using System.Web.Mvc;
using Abp.Dependency;
using Cloaked;
using DefaultModelBinder = System.Web.Mvc.DefaultModelBinder;
using ModelBindingContext = System.Web.Mvc.ModelBindingContext;

namespace Web.ModelBinding;

internal sealed class CloakMvcModelBinder : DefaultModelBinder, ISingletonDependency
{
    private readonly ICloak _cloak;

    public CloakMvcModelBinder(ICloak cloak)
    {
        _cloak = cloak;
    }

    public override object BindModel(
        ControllerContext controllerContext,
        ModelBindingContext bindingContext
    )
    {
        // Get metadata for the property being bound
        var propertyInfo = bindingContext.ModelMetadata.ContainerType?.GetProperty(
            bindingContext.ModelMetadata.PropertyName ?? string.Empty
        );

        if (propertyInfo is null || !Attribute.IsDefined(propertyInfo, typeof(CloakAttribute)))
            return base.BindModel(controllerContext, bindingContext);

        // Get the raw value from request
        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueResult == null || string.IsNullOrEmpty(valueResult.AttemptedValue))
            return base.BindModel(controllerContext, bindingContext);

        // Determine actual type (handle nullable)
        var modelType =
            Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;

        try
        {
            return _cloak.Decode(valueResult.AttemptedValue, modelType);
        }
        catch
        {
            // Fallback to base binder if decoding fails
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}

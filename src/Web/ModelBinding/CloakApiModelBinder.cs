using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Abp.Dependency;
using Cloaked;

namespace Web.ModelBinding;

internal sealed class CloakApiModelBinder : IModelBinder, ISingletonDependency
{
    private readonly ICloak _cloak;

    public CloakApiModelBinder(ICloak cloak)
    {
        _cloak = cloak;
    }

    public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        // Get property info (same logic as MVC binder)
        var propertyInfo = bindingContext.ModelMetadata.ContainerType?.GetProperty(
            bindingContext.ModelMetadata.PropertyName ?? string.Empty
        );

        // If no property or no [Cloak] attribute, let default binder handle it
        if (propertyInfo == null || !Attribute.IsDefined(propertyInfo, typeof(CloakAttribute)))
            return false;

        // Get raw value from value provider
        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueResult == null || string.IsNullOrEmpty(valueResult.AttemptedValue))
            return false;

        var stringValue = valueResult.AttemptedValue;

        // Determine target type
        var modelType =
            Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;

        try
        {
            var decodedValue = _cloak.Decode(stringValue, modelType);
            bindingContext.Model = decodedValue;
            return true;
        }
        catch
        {
            // Fail gracefully — let other binders try
            return false;
        }
    }
}

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Cloaked;

/// <summary>
/// JSON converter modifier that handles properties marked with [Cloak] attribute.
/// </summary>
public class CloakTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    private readonly ICloak _codec;

    /// <summary>
    /// JSON converter modifier that handles properties marked with [Cloak] attribute.
    /// </summary>
    public CloakTypeInfoResolver(ICloak codec)
    {
        _codec = codec;
    }

    /// <summary>
    /// Gets the type information for the specified type, adding Cloak custom converters for properties marked with [Cloak].
    /// </summary>
    /// <param name="type">The type to get information for.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <returns>The JSON type information with Cloak converters applied where appropriate.</returns>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
        {
            foreach (var propertyInfo in jsonTypeInfo.Properties)
            {
                var propertyType = propertyInfo.PropertyType;
                var property = type.GetProperty(
                    propertyInfo.Name,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (
                    property?.GetCustomAttribute<CloakAttribute>() != null
                    && IsNumericType(propertyType)
                )
                {
                    propertyInfo.CustomConverter = new CloakPropertyConverter(propertyType, _codec);
                }
            }
        }

        return jsonTypeInfo;
    }

    private static bool IsNumericType(Type type)
    {
        var actualType = Nullable.GetUnderlyingType(type) ?? type;
        return actualType == typeof(int);
    }
}

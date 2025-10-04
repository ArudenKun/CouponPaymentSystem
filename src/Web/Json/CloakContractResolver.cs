using System.Reflection;
using Abp.Dependency;
using Cloaked;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Json;

public class CloakContractResolver : DefaultContractResolver, ISingletonDependency
{
    private readonly ICloak _cloak;

    public CloakContractResolver(ICloak cloak)
    {
        _cloak = cloak;
    }

    protected override JsonProperty CreateProperty(
        MemberInfo member,
        MemberSerialization memberSerialization
    )
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (member is PropertyInfo propInfo)
        {
            // Check if [Cloak] is applied
            if (
                propInfo.GetCustomAttribute<CloakAttribute>() != null
                && IsNumericType(propInfo.PropertyType)
            )
            {
                property.Converter = new CloakNewtonsoftPropertyConverter(
                    propInfo.PropertyType,
                    _cloak
                );
            }
        }

        return property;
    }

    private static bool IsNumericType(Type type)
    {
        var actualType = Nullable.GetUnderlyingType(type) ?? type;
        return actualType == typeof(int)
            || actualType == typeof(uint)
            || actualType == typeof(long)
            || actualType == typeof(ulong)
            || actualType == typeof(short)
            || actualType == typeof(ushort);
    }
}

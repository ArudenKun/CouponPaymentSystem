using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Abp.Auditing;

/// <summary>
/// Decides which properties of auditing class to be serialized
/// </summary>
public class AuditingJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    private readonly List<Type> _ignoredTypes;

    public AuditingJsonTypeInfoResolver(List<Type> ignoredTypes)
    {
        _ignoredTypes = ignoredTypes;
    }

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
        {
            foreach (var prop in jsonTypeInfo.Properties)
            {
                var member = prop.AttributeProvider as MemberInfo;
                if (member == null)
                    continue;

                // Ignore if DisableAuditing or JsonIgnore is applied
                if (
                    member.IsDefined(typeof(DisableAuditingAttribute))
                    || member.IsDefined(typeof(JsonIgnoreAttribute))
                )
                {
                    prop.ShouldSerialize = (_, _) => false;
                    continue;
                }

                // Ignore if type is in ignored list
                foreach (var ignoredType in _ignoredTypes)
                {
                    if (ignoredType.GetTypeInfo().IsAssignableFrom(prop.PropertyType))
                    {
                        prop.ShouldSerialize = (_, _) => false;
                        break;
                    }
                }
            }
        }

        return jsonTypeInfo;
    }
}

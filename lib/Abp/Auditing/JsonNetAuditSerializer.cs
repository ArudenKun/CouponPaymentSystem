using System.Text.Json;
using Abp.Dependency;

namespace Abp.Auditing;

public class JsonNetAuditSerializer : IAuditSerializer, ITransientDependency
{
    private readonly IAuditingConfiguration _configuration;

    public JsonNetAuditSerializer(IAuditingConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Serialize(object obj)
    {
        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = new AuditingJsonTypeInfoResolver(_configuration.IgnoredTypes),
        };

        return JsonSerializer.Serialize(obj, options);
    }
}

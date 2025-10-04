using Abp.Dependency;
using Abp.Json;
using Cloaked;
using Newtonsoft.Json.Serialization;

namespace Web.Json;

public sealed class CpsWebContractResolver : IContractResolver, ISingletonDependency
{
    private readonly CloakContractResolver _cloakContractResolver;

    private readonly AbpCamelCasePropertyNamesContractResolver _abpCamelCasePropertyNamesContractResolver =
        new();

    public CpsWebContractResolver(CloakContractResolver cloakContractResolver)
    {
        _cloakContractResolver = cloakContractResolver;
    }

    public JsonContract ResolveContract(Type type) =>
        type.IsDefined(typeof(CloakAttribute), true)
            ? _cloakContractResolver.ResolveContract(type)
            : _abpCamelCasePropertyNamesContractResolver.ResolveContract(type);
}

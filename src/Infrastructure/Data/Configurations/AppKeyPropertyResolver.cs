using Dommel;

namespace Infrastructure.Data.Configurations;

public class AppKeyPropertyResolver : IKeyPropertyResolver
{
    private static readonly IKeyPropertyResolver DefaultKeyPropertyResolver =
        new DefaultKeyPropertyResolver();

    public ColumnPropertyInfo[] ResolveKeyProperties(Type type)
    {
        var keyProps = Resolvers
            .Properties(type)
            .Select(p => p.Property)
            .Where(p => p.Name == $"{type.Name}Id")
            .ToArray();

        return keyProps.Length > 0
            ? keyProps.Select(p => new ColumnPropertyInfo(p, true)).ToArray()
            : DefaultKeyPropertyResolver.ResolveKeyProperties(type);
    }
}

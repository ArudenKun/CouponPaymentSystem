using System.Reflection;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Linq2Db.Common;
using Abp.Linq2Db.Utilities;
using LinqToDB;
using LinqToDB.Data;

namespace Abp.Linq2Db;

internal sealed class Linq2DbContextEntityFinder : IDbContextEntityFinder, ITransientDependency
{
    public IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
    {
        DataConnection connection;
        return from property in dbContextType.GetProperties(
                BindingFlags.Public | BindingFlags.Instance
            )
            where
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(ITable<>))
                && ReflectionHelper.IsAssignableToGenericType(
                    property.PropertyType.GenericTypeArguments[0],
                    typeof(IEntity<>)
                )
            select new EntityTypeInfo(
                property.PropertyType.GenericTypeArguments[0],
                property.DeclaringType
            );
    }
}

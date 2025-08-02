using Abp.Domain.Entities;

namespace Abp.Linq2Db.Common;

public interface IDbContextEntityFinder
{
    IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
}

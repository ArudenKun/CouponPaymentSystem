namespace Abp.Linq2Db.Common;

public interface IDbContextTypeMatcher
{
    void Populate(Type[] dbContextTypes);

    Type GetConcreteType(Type sourceDbContextType);
}

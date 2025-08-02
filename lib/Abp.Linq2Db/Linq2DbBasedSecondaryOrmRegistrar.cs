using Abp.Linq2Db.Common;

namespace Abp.Linq2Db;

public class Linq2DbBasedSecondaryOrmRegistrar : SecondaryOrmRegistrarBase
{
    public Linq2DbBasedSecondaryOrmRegistrar(
        Type dbContextType,
        IDbContextEntityFinder dbContextEntityFinder
    )
        : base(dbContextType, dbContextEntityFinder) { }

    public override string OrmContextKey => "Linq2Db";
}

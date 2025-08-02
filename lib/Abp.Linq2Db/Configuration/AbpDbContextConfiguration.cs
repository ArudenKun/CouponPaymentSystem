using LinqToDB;
using LinqToDB.Data;

namespace Abp.Linq2Db.Configuration;

public class AbpDbContextConfiguration<TDbContext>
    where TDbContext : DataConnection
{
    public DataOptions<TDbContext> DataOptions { get; }

    public AbpDbContextConfiguration()
    {
        DataOptions = new DataOptions<TDbContext>(new DataOptions());
    }
}

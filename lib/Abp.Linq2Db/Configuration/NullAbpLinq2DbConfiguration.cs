using LinqToDB.Data;

namespace Abp.Linq2Db.Configuration;

public class NullAbpLinq2DbConfiguration : IAbpLinq2DbConfiguration
{
    /// <summary>
    /// Gets single instance of <see cref="NullAbpLinq2DbConfiguration"/> class.
    /// </summary>
    public static NullAbpLinq2DbConfiguration Instance { get; } = new();

    public bool UseAbpQueryCompiler { get; set; }

    public void AddDbContext<TDbContext>(Action<AbpDbContextConfiguration<TDbContext>> action)
        where TDbContext : DataConnection { }
}

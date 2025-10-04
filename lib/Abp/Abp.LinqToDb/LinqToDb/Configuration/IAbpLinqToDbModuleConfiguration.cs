using LinqToDB;

namespace Abp.LinqToDb.Configuration;

public interface IAbpLinqToDbModuleConfiguration
{
    DataOptions DataOptions { get; }
}

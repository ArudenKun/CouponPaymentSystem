using JetBrains.Annotations;

namespace Abp.Linq2Db.Common;

[PublicAPI]
public interface IShouldInitializeDbContext
{
    void Initialize(AbpLinq2DbContextInitializationContext initializationContext);
}

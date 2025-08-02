using Abp.Domain.Uow;

namespace Abp.Linq2Db.Common;

public class AbpLinq2DbContextInitializationContext
{
    public IUnitOfWork UnitOfWork { get; }

    public AbpLinq2DbContextInitializationContext(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}

using NHibernate;

namespace Abp.NHibernate.Repositories;

public interface IRepositoryWithSession
{
    ISession GetSession();

    Task<ISession> GetSessionAsync();
}

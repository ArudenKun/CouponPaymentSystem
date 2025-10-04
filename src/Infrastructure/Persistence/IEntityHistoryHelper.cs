using NHibernate.Event;

namespace Infrastructure.Persistence;

internal interface IEntityHistoryHelper
{
    void AddEntityToChangeSet(AbstractPreDatabaseOperationEvent @event);

    void SaveChangeSet(Guid sessionId);
}

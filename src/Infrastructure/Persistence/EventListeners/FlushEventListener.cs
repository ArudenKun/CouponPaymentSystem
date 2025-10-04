using NHibernate.Event;
using NHibernate.Event.Default;

namespace Infrastructure.Persistence.EventListeners;

internal class FlushEventListener : DefaultFlushEventListener
{
    private readonly IEntityHistoryHelper _entityHistoryHelper;

    public FlushEventListener(IEntityHistoryHelper entityHistoryHelper)
    {
        _entityHistoryHelper = entityHistoryHelper;
    }

    public override void OnFlush(FlushEvent @event)
    {
        _entityHistoryHelper.SaveChangeSet(@event.Session.SessionId);
        base.OnFlush(@event);
    }

    public override Task OnFlushAsync(FlushEvent @event, CancellationToken cancellationToken)
    {
        _entityHistoryHelper.SaveChangeSet(@event.Session.SessionId);
        return base.OnFlushAsync(@event, cancellationToken);
    }
}

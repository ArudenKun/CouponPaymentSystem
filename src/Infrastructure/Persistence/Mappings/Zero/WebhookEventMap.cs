using Abp.NHibernate.EntityMappings;
using Abp.Webhooks;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class WebhookEventMap : EntityMap<WebhookEvent, Guid>
{
    public WebhookEventMap()
        : base("AbpWebhookEvents")
    {
        Map(x => x.WebhookName).Length(Extensions.NvarcharMax).Not.Nullable();
        Map(x => x.Data).Length(Extensions.NvarcharMax).Nullable();
        this.MapCreationTime();
        Map(x => x.TenantId);
        this.MapIsDeleted();
        this.Map(x => x.DeletionTime);
    }
}

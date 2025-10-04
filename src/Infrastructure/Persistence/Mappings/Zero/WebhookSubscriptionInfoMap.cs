using Abp.NHibernate.EntityMappings;
using Abp.Webhooks;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class WebhookSubscriptionInfoMap : EntityMap<WebhookSubscriptionInfo, Guid>
{
    public WebhookSubscriptionInfoMap()
        : base("AbpWebhookSubscriptions")
    {
        this.MapCreationAudited();

        Map(x => x.TenantId);
        Map(x => x.WebhookUri).Length(Extensions.NvarcharMax).Not.Nullable();
        Map(x => x.Secret).Length(Extensions.NvarcharMax).Not.Nullable();
        this.MapIsActive();
        Map(x => x.Webhooks).Length(Extensions.NvarcharMax);
        Map(x => x.Headers).Length(Extensions.NvarcharMax);
    }
}

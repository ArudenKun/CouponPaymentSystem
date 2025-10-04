using Abp.Configuration;
using Abp.NHibernate.EntityMappings;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class SettingMap : EntityMap<Setting, long>
{
    public SettingMap()
        : base("AbpSettings")
    {
        Map(x => x.TenantId);
        Map(x => x.UserId);
        Map(x => x.Name).Length(Setting.MaxNameLength).Not.Nullable();
        Map(x => x.Value).Length(Extensions.NvarcharMax);

        this.MapAudited();
    }
}

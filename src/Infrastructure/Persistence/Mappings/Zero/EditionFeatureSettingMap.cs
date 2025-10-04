using Abp.Application.Features;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class EditionFeatureSettingMap : SubclassMap<EditionFeatureSetting>
{
    public EditionFeatureSettingMap()
    {
        DiscriminatorValue("EditionFeatureSetting");

        References(x => x.Edition).Column("EditionId");
        Map(x => x.EditionId).ReadOnly();
    }
}

using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class UserClaimMap : EntityMap<UserClaim, long>
{
    public UserClaimMap()
        : base("AbpUserClaims")
    {
        Map(x => x.TenantId);
        Map(x => x.UserId).Not.Nullable();
        Map(x => x.ClaimType).Length(UserClaim.MaxClaimTypeLength);
        Map(x => x.ClaimValue).Length(Extensions.NvarcharMax);

        this.MapCreationAudited();
    }
}

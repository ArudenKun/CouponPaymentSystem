using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;

namespace Infrastructure.Persistence.Mappings.Zero;

internal class UserAccountMap : EntityMap<UserAccount, long>
{
    public UserAccountMap()
        : base("AbpUserAccounts")
    {
        Map(x => x.TenantId);
        Map(x => x.UserId).Not.Nullable();
        Map(x => x.UserLinkId);
        Map(x => x.UserName).Length(UserAccount.MaxUserNameLength);
        Map(x => x.EmailAddress).Length(UserAccount.MaxEmailAddressLength);

        this.MapFullAudited();
    }
}

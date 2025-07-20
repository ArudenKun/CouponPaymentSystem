using Abp.NHibernate.EntityMappings;
using Abp.Notifications;

namespace CouponPaymentSystem.Core.Notifications;

public class UserNotificationInfoMap : EntityMap<UserNotificationInfo, Guid>
{
    public UserNotificationInfoMap()
        : base(nameof(UserNotificationInfo))
    {
        Map(x => x.State).CustomType<UserNotificationState>().Not.Nullable();
        Map(x => x.TenantId);
        Map(x => x.TenantNotificationId).Not.Nullable();
        Map(x => x.UserId).Not.Nullable();
        this.MapCreationTime();
    }
}

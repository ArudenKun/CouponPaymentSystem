﻿using Abp.NHibernate.EntityMappings;
using Abp.Notifications;

namespace CouponPaymentSystem.Core.Notifications;

public class NotificationSubscriptionInfoMap : EntityMap<NotificationSubscriptionInfo, Guid>
{
    public NotificationSubscriptionInfoMap()
        : base(nameof(NotificationSubscriptionInfo))
    {
        Map(x => x.EntityId).Length(NotificationInfo.MaxEntityIdLength);
        Map(x => x.EntityTypeAssemblyQualifiedName)
            .Length(NotificationInfo.MaxEntityTypeAssemblyQualifiedNameLength);
        Map(x => x.EntityTypeName).Length(NotificationInfo.MaxEntityTypeNameLength);
        Map(x => x.NotificationName).Length(NotificationInfo.MaxNotificationNameLength);
        Map(x => x.TenantId);
        Map(x => x.UserId).Not.Nullable();

        this.MapCreationAudited();
    }
}

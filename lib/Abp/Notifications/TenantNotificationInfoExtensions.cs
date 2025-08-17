using System.Text.Json;
using Abp.Domain.Entities;
using Abp.Extensions;

namespace Abp.Notifications;

/// <summary>
/// Extension methods for <see cref="NotificationInfo"/>.
/// </summary>
public static class TenantNotificationInfoExtensions
{
    /// <summary>
    /// Converts <see cref="NotificationInfo"/> to <see cref="TenantNotification"/>.
    /// </summary>
    public static TenantNotification ToTenantNotification(
        this TenantNotificationInfo tenantNotificationInfo
    )
    {
        var entityType = tenantNotificationInfo.EntityTypeAssemblyQualifiedName.IsNullOrEmpty()
            ? null
            : Type.GetType(tenantNotificationInfo.EntityTypeAssemblyQualifiedName);

        return new TenantNotification
        {
            Id = tenantNotificationInfo.Id,
            TenantId = tenantNotificationInfo.TenantId,
            NotificationName = tenantNotificationInfo.NotificationName,
            Data = tenantNotificationInfo.Data.IsNullOrEmpty()
                ? null
                : JsonSerializer.Deserialize(
                    tenantNotificationInfo.Data,
                    Type.GetType(tenantNotificationInfo.DataTypeName)!
                ) as NotificationData,
            EntityTypeName = tenantNotificationInfo.EntityTypeName,
            EntityId = tenantNotificationInfo.EntityId.IsNullOrEmpty()
                ? null
                : JsonSerializer.Deserialize(
                    tenantNotificationInfo.EntityId,
                    EntityHelper.GetPrimaryKeyType(entityType)
                ),
            Severity = tenantNotificationInfo.Severity,
            CreationTime = tenantNotificationInfo.CreationTime,
        };
    }
}

using Domain.Tenants;
using Domain.Users;
using Infrastructure.Persistence.Mappings.Zero;

namespace Infrastructure.Persistence.Mappings;

internal class TenantMap : AbpTenantMap<Tenant, User>;

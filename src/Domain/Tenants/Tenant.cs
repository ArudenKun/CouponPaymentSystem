using Abp.MultiTenancy;
using Domain.Users;

namespace Domain.Tenants;

public class Tenant : AbpTenant<User> { }

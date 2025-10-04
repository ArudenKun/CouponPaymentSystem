﻿using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Domain.Roles;
using Domain.Tenants;
using Domain.Users;

namespace Domain.Authorization;

public sealed class LogInManager : AbpLogInManager<Tenant, Role, User>
{
    public LogInManager(
        UserManager userManager,
        IMultiTenancyConfig multiTenancyConfig,
        IRepository<Tenant> tenantRepository,
        IUnitOfWorkManager unitOfWorkManager,
        ISettingManager settingManager,
        IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
        IUserManagementConfig userManagementConfig,
        IIocResolver iocResolver,
        RoleManager roleManager
    )
        : base(
            userManager,
            multiTenancyConfig,
            tenantRepository,
            unitOfWorkManager,
            settingManager,
            userLoginAttemptRepository,
            userManagementConfig,
            iocResolver,
            roleManager
        ) { }

    public override async Task<AbpLoginResult<Tenant, User>> LoginAsync(
        string userNameOrEmailAddress,
        string plainPassword,
        string? tenancyName = null,
        bool shouldLockout = true
    )
    {
        return await UnitOfWorkManager.WithUnitOfWorkAsync(async () =>
        {
            var result = await LoginAsyncInternal(
                userNameOrEmailAddress,
                plainPassword,
                tenancyName,
                shouldLockout
            );
            await SaveLoginAttempt(result, tenancyName, userNameOrEmailAddress);
            return result;
        });
    }
}

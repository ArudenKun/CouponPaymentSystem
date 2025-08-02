using System.Reflection;
using Abp.AutoMapper;
using Abp.FluentValidation;
using Abp.Modules;
using CouponPaymentSystem.Application.Authorization;
using CouponPaymentSystem.Application.Common.Extensions;
using CouponPaymentSystem.Domain;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace CouponPaymentSystem.Application;

[DependsOn(typeof(CpsDomainModule), typeof(AbpAutoMapperModule), typeof(AbpFluentValidationModule))]
public class CpsApplicationModule : AbpModule
{
    private static readonly Assembly ThisAssembly = typeof(CpsApplicationModule).Assembly;

    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<AuthorizationProvider>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(ThisAssembly);
        IocManager.RegisterWithServiceCollection(services =>
            services
                .AddMediator(options =>
                {
                    options.GenerateTypesAsInternal = true;
                    options.ServiceLifetime = ServiceLifetime.Transient;
                })
                .AddSingleton(new SqidsEncoder(new SqidsOptions { MinLength = 8 }))
        );
        Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(ThisAssembly));
    }
}

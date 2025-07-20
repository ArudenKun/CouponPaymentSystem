using Abp.AutoMapper;
using Abp.BlobStoring;
using Abp.FluentValidation;
using Abp.Modules;
using CouponPaymentSystem.Core;
using CouponPaymentSystem.Core.Common.Extensions;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Sqids;
using ZiggyCreatures.Caching.Fusion;

namespace CouponPaymentSystem.Application;

[DependsOn(
    typeof(CpsCoreModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpFluentValidationModule),
    typeof(AbpBlobStoringModule)
)]
public class CpsApplicationModule : AbpModule
{
    public override void Initialize()
    {
        var thisAssembly = typeof(CpsApplicationModule).Assembly;
        IocManager.RegisterAssemblyByConvention(thisAssembly);
        IocManager.Register();
        IocManager.WithServiceCollection(services =>
            services
                .AddSingleton(new SqidsEncoder(new SqidsOptions { MinLength = 8 }))
                .AddFusionCache()
                .WithDefaultEntryOptions(options => options.SetDuration(30.Minutes()))
        );
        Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(thisAssembly));
    }
}

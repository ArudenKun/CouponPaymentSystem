using System.Reflection;
using Abp;
using Abp.Modules;
using Abp.Runtime.Security;

namespace CouponPaymentSystem.Domain;

[DependsOn(typeof(AbpKernelModule))]
public class CpsDomainModule : AbpModule
{
    private static readonly Assembly ThisAssembly = typeof(CpsDomainModule).Assembly;

    public override void PreInitialize()
    {
        Configuration.MultiTenancy.IsEnabled = CpsConstants.MultiTenancyEnabled;
        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase =
            CpsConstants.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = CpsConstants.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(ThisAssembly);
    }
}

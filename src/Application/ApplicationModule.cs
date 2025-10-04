using Abp.Modules;
using Domain;

namespace Application;

[DependsOn(typeof(DomainModule))]
public class ApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        base.PreInitialize();
    }
}

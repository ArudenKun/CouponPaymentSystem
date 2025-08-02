using Abp.Modules;
using Abp.Reflection.Extensions;
using JetBrains.Annotations;

namespace Abp.Linq2Db.Common;

[PublicAPI]
[DependsOn(typeof(AbpKernelModule))]
public class AbpLinq2DbCommonModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(AbpLinq2DbCommonModule).GetAssembly());
    }
}

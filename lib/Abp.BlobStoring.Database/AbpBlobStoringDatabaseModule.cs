using Abp.Modules;

namespace Abp.BlobStoring.Database;

[DependsOn(typeof(AbpBlobStoringModule))]
public class AbpBlobStoringDatabaseModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration
            .Modules.AbpBlobStoring()
            .Containers.ConfigureDefault(options =>
            {
                if (options.ProviderType is null)
                {
                    options.UseDatabase();
                }
            });
    }
}

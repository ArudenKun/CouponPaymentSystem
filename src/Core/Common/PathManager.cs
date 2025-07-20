using Abp.Domain.Services;
using AutoInterfaceAttributes;
using CouponPaymentSystem.Core.Common.Extensions;

namespace CouponPaymentSystem.Core.Common;

[AutoInterface(Inheritance = [typeof(IDomainService)])]
public class PathManager : IPathManager
{
    private static readonly Lazy<string> RootDirLazy = new(() =>
        AppDomain.CurrentDomain.BaseDirectory
    );

    public string RootDir => RootDirLazy.Value;

    public string AppDataDir => RootDir.Combine("App_Data");
    public string BlobsDir => AppDataDir.Combine("Blobs");
    public string ConfigPath => RootDir.Combine("config.json");
}

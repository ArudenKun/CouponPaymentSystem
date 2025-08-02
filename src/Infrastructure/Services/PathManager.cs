using CouponPaymentSystem.Application.Common;
using CouponPaymentSystem.Application.Common.Extensions;

namespace CouponPaymentSystem.Infrastructure.Services;

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

namespace CouponPaymentSystem.Application.Common;

public interface IPathManager
{
    string RootDir { get; }
    string AppDataDir { get; }
    string BlobsDir { get; }
    string ConfigPath { get; }
}

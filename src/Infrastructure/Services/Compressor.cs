using Abp.Dependency;
using Aoxe.Extensions;
using Aoxe.SystemIoCompression;
using CouponPaymentSystem.Application.Common;

namespace CouponPaymentSystem.Infrastructure.Services;

internal sealed class Compressor : ICompressor, ISingletonDependency
{
    public string Compress(string value) =>
        string.IsNullOrWhiteSpace(value) ? value : value.ToGZip().ToBase64String();

    public string Decompress(string base64) =>
        string.IsNullOrWhiteSpace(base64) ? base64 : base64.FromBase64ToBytes().UnGZipToString();
}

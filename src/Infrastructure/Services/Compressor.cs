using System.IO.Compression;
using System.Text;
using Abp.Dependency;
using Abp.Extensions;
using CouponPaymentSystem.Application.Common;

namespace CouponPaymentSystem.Infrastructure.Services;

internal sealed class Compressor : ICompressor, ISingletonDependency
{
    public string Compress(string value)
    {
        if (value.IsNullOrWhiteSpace())
            return value;

        var bytes = Encoding.UTF8.GetBytes(value);

        using var inputStream = new MemoryStream(bytes);
        using var outputStream = new MemoryStream();

        Compress(inputStream, outputStream);

        return Convert.ToBase64String(outputStream.ToArray());
    }

    public static void Compress(
        Stream inputStream,
        Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal
    )
    {
        using var gzipOutputStream = new GZipStream(
            outputStream,
            compressionLevel,
            leaveOpen: true
        );
        inputStream.CopyTo(gzipOutputStream);
    }

    public string Decompress(string base64)
    {
        if (base64.IsNullOrWhiteSpace())
            return base64;

        var bytes = Convert.FromBase64String(base64);

        using var inputStream = new MemoryStream(bytes);
        using var outputStream = new MemoryStream();

        Decompress(inputStream, outputStream);

        return Encoding.UTF8.GetString(outputStream.ToArray());
    }

    public static void Decompress(Stream inputStream, Stream outputStream)
    {
        using var gzipInputStream = new GZipStream(
            inputStream,
            CompressionMode.Decompress,
            leaveOpen: true
        );
        gzipInputStream.CopyTo(outputStream);
    }
}

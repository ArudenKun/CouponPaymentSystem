namespace CouponPaymentSystem.Application.Common;

public interface ICompressor
{
    string Compress(string value);

    string Decompress(string value);
}

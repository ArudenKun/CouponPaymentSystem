namespace CouponPaymentSystem.Application.Common.Extensions;

public static class StringExtensions
{
    public static string PadLeftZero(this string value, int totalWidth) =>
        string.IsNullOrWhiteSpace(value)
            ? new string('0', totalWidth)
            : value.PadLeft(totalWidth, '0');

    public static string PadLeftZero(this long value, int totalWidth) =>
        $"{value}".PadLeftZero(totalWidth);

    public static string PadLeftZero(this int value, int totalWidth) =>
        ((long)value).PadLeftZero(totalWidth);
}

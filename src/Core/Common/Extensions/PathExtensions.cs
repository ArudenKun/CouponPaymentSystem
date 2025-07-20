namespace CouponPaymentSystem.Core.Common.Extensions;

public static class PathExtensions
{
    public static string Combine(this string path, params string[] parts)
    {
        var paths = new List<string> { path };
        paths.AddRange(parts);
        return Path.Combine([.. paths]);
    }
}

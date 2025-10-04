using System.Text.RegularExpressions;

namespace Web.Utilities;

public static class UrlChecker
{
    private static readonly Regex UrlWithProtocolRegex = new Regex("^.{1,10}://.*$");

    public static bool IsRooted(string url)
    {
        if (url.StartsWith("/") || url.StartsWith("~/"))
        {
            return true;
        }

        return UrlWithProtocolRegex.IsMatch(url);
    }
}

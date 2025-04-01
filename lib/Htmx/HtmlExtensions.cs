using System.Web.Mvc;

namespace Htmx;

public static class HtmlExtensions
{
    /// <summary>
    /// Adds an event handler for "htmx:configRequest" to hydrate
    /// an antiforgery token on each HTMX mutating request (POST, PUT, DELETE).
    /// </summary>
    /// <remarks>
    /// Note: This includes wrapping script tags. To get the JavaScript string
    /// use <see cref="HtmxSnippets.AntiforgeryJavaScript">HtmxSnippets.AntiforgeryJavaScript</see>.
    /// </remarks>
    /// <param name="helper">An instance of the HTML Helper interface</param>
    /// <param name="minified">Determines if the JavaScript is minified (defaults to true)</param>
    /// <returns>HTML Content with JavaScript tag</returns>
    public static MvcHtmlString HtmxAntiforgeryScript(this HtmlHelper helper, bool minified = true)
    {
        var javaScript = HtmxSnippets.GetAntiforgeryJavaScript(minified);
        return new MvcHtmlString($"<script>{javaScript}</script>");
    }
}

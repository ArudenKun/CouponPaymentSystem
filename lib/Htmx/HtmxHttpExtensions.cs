using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Htmx;

public static class HtmxHttpExtensions
{
    /// <summary>
    /// Determines if the current HTTP Request was invoked by Htmx on the client
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static bool IsHtmx(this HttpRequestBase? request)
    {
        return request?.Headers.ContainsKey(HtmxRequestHeaders.Keys.Request) is true;
    }

    /// <summary>
    /// Determines if the current HTTP Request was invoked by Htmx on the client
    /// </summary>
    /// <param name="request">The HTTP Request</param>
    /// <param name="values">All the potential Htmx Header Values</param>
    /// <returns></returns>
    public static bool IsHtmx(this HttpRequestBase? request, out HtmxRequestHeaders? values)
    {
        var isHtmx = request.IsHtmx();
        values = request is not null && isHtmx ? new HtmxRequestHeaders(request) : null;
        return isHtmx;
    }

    /// <summary>
    /// true if the request is for history restoration after a miss in the local history cache
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static bool IsHtmxHistoryRestoreRequest(this HttpRequestBase? request)
    {
        return request?.Headers.GetValueOrDefault(
            HtmxRequestHeaders.Keys.HistoryRestoreRequest,
            false
        )
            is true;
    }

    /// <summary>
    /// true if the request is an HTMX Boosted Request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static bool IsHtmxBoosted(this HttpRequestBase? request)
    {
        return request?.Headers.GetValueOrDefault(HtmxRequestHeaders.Keys.Boosted, false) is true;
    }

    /// <summary>
    /// true if the request is an HTMX Request that is not HTMX Boosted
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static bool IsHtmxNonBoosted(this HttpRequestBase? request)
    {
        return request?.IsHtmx() is true && !request.IsHtmxBoosted();
    }

    /// <summary>
    /// true if the request is an HTMX Request that is not HTMX Boosted
    /// </summary>
    /// <param name="request">The HTTP Request</param>
    /// <param name="values">All the potential Htmx Header Values</param>
    /// <returns></returns>
    public static bool IsHtmxNonBoosted(
        this HttpRequestBase? request,
        out HtmxRequestHeaders? values
    )
    {
        var isHtmx = request.IsHtmxNonBoosted();
        values = request is not null && isHtmx ? new HtmxRequestHeaders(request) : null;
        return isHtmx;
    }

    /// <summary>
    /// Set the Htmx Response Headers
    /// </summary>
    /// <param name="response"></param>
    /// <param name="action">Action used to action the response headers</param>
    public static void Htmx(this HttpResponseBase response, Action<HtmxResponseHeaders> action)
    {
        var headerContainer = new HtmxResponseHeaders(response.Headers);
        action(headerContainer);
        headerContainer.Process();
    }

    internal static T GetValueOrDefault<T>(this NameValueCollection headers, string key, T @default)
    {
        return headers.TryGetValue(key, out var values)
            ? (T)Convert.ChangeType(values.First(), typeof(T))
            : @default;
    }

    // Case-insensitive ContainsKey
    internal static bool ContainsKey(
        this NameValueCollection collection,
        string key,
        bool caseSensitive = false
    ) =>
        collection.AllKeys.Contains(
            key,
            caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase
        );

    /// <summary>
    /// Attempts to get the value associated with the specified key.
    /// </summary>
    /// <param name="collection">The NameValueCollection</param>
    /// <param name="key">The key of the value to get</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if found; otherwise null</param>
    /// <returns>true if the collection contains an element with the specified key; otherwise, false</returns>
    internal static bool TryGetValue(
        this NameValueCollection collection,
        string key,
        out string value
    )
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        value = collection[key];
        return value != null;
    }

    // /// <summary>
    // /// Attempts to get and convert the value associated with the specified key.
    // /// </summary>
    // /// <typeparam name="T">The type to convert the value to</typeparam>
    // /// <param name="collection">The NameValueCollection</param>
    // /// <param name="key">The key of the value to get</param>
    // /// <param name="value">When this method returns, contains the converted value if found; otherwise default(T)</param>
    // /// <returns>true if the collection contains an element with the specified key and conversion succeeded; otherwise, false</returns>
    // internal static bool TryGetValue<T>(
    //     this NameValueCollection collection,
    //     string key,
    //     out T value
    // )
    // {
    //     value = default;
    //
    //     if (!collection.TryGetValue(key, out string stringValue))
    //         return false;
    //
    //     try
    //     {
    //         value = (T)Convert.ChangeType(stringValue, typeof(T));
    //         return true;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }
}

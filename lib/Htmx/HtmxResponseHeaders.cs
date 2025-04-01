using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Htmx;

/// <summary>
/// Htmx Response Headers
/// https://htmx.org/reference/#response_headers
/// </summary>
public class HtmxResponseHeaders
{
    private readonly NameValueCollection _headers;

    private readonly Dictionary<HtmxTriggerTiming, Dictionary<string, object?>> _triggers = new();

    public static class Keys
    {
        // Sorted by https://htmx.org/reference/#response_headers to make it easier to update
        public const string Location = "HX-Location";
        public const string PushUrl = "HX-Push-Url";
        public const string Redirect = "HX-Redirect";
        public const string Refresh = "HX-Refresh";
        public const string ReplaceUrl = "HX-Replace-Url";
        public const string Reswap = "HX-Reswap";
        public const string Retarget = "HX-Retarget";
        public const string Reselect = "HX-Reselect";
        public const string Trigger = "HX-Trigger";
        public const string TriggerAfterSettle = "HX-Trigger-After-Settle";
        public const string TriggerAfterSwap = "HX-Trigger-After-Swap";

        public static string[] All { get; } =
            [
                Location,
                PushUrl,
                Redirect,
                Refresh,
                ReplaceUrl,
                Reswap,
                Retarget,
                Reselect,
                Trigger,
                TriggerAfterSettle,
                TriggerAfterSwap,
            ];
    }

    internal HtmxResponseHeaders(NameValueCollection headers)
    {
        _headers = headers;
    }

    /// <summary>
    ///	pushes a new url into the history stack
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("HX-Push was replaced with HX-Push-Url, use PushUrl(value) instead")]
    public HtmxResponseHeaders Push(string value)
    {
        return PushUrl(value);
    }

    /// <summary>
    ///	pushes a new url into the history stack
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public HtmxResponseHeaders PushUrl(string value)
    {
        _headers[Keys.PushUrl] = value;
        return this;
    }

    /// <summary>
    /// Can be used to do a client-side redirect to a new location
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public HtmxResponseHeaders Redirect(string value)
    {
        _headers[Keys.Redirect] = value;
        return this;
    }

    /// <summary>
    /// Allows you to specify how the response will be swapped
    /// See https://htmx.org/attributes/hx-swap/ for values
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public HtmxResponseHeaders Reswap(string value)
    {
        _headers[Keys.Reswap] = value;
        return this;
    }

    /// <summary>
    /// a CSS selector that allows you to choose which part of the response is used to be swapped in.
    /// Overrides an existing hx-select on the triggering element
    /// See https://htmx.org/attributes/hx-select/ for hx-select behavior.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public HtmxResponseHeaders Reselect(string value)
    {
        _headers[Keys.Reselect] = value;
        return this;
    }

    /// <summary>
    /// Allows you to do a client-side redirect that does not do a full page reload
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public HtmxResponseHeaders Location(string value)
    {
        _headers[Keys.Location] = value;
        return this;
    }

    /// <summary>
    /// Replaces the current URL in the location bar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public HtmxResponseHeaders ReplaceUrl(string value)
    {
        _headers[Keys.ReplaceUrl] = value;
        return this;
    }

    /// <summary>
    /// if called the client side will do a a full refresh of the page
    /// </summary>
    /// <returns></returns>
    public HtmxResponseHeaders Refresh()
    {
        _headers[Keys.Refresh] = "true";
        return this;
    }

    /// <summary>
    /// allows you to trigger client side events
    /// https://htmx.org/headers/hx-trigger
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("WithTrigger(name, val?, timing?) provides more options for configuring triggers.")]
    public HtmxResponseHeaders Trigger(string value)
    {
        _headers[Keys.Trigger] = value;
        return this;
    }

    /// <summary>
    /// allows you to trigger client side events after a settle
    /// https://htmx.org/headers/hx-trigger
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("WithTrigger(name, val?, timing?) provides more options for configuring triggers.")]
    public HtmxResponseHeaders TriggerAfterSettle(string value)
    {
        _headers[Keys.TriggerAfterSettle] = value;
        return this;
    }

    /// <summary>
    /// allows you to trigger client side events after a swap
    /// https://htmx.org/headers/hx-trigger
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("WithTrigger(name, val?, timing?) provides more options for configuring triggers.")]
    public HtmxResponseHeaders TriggerAfterSwap(string value)
    {
        _headers[Keys.TriggerAfterSwap] = value;
        return this;
    }

    /// <summary>
    /// A header that allows you to change the default target of returned content
    /// </summary>
    /// <param name="value">CSS Selector of target</param>
    /// <returns></returns>
    public HtmxResponseHeaders Retarget(string value)
    {
        _headers[Keys.Retarget] = value;
        return this;
    }

    /// <summary>
    /// Trigger a client side event named <paramref name="eventName"/> at different stages of the HTMX lifecycle.
    /// </summary>
    /// <param name="eventName">Event name</param>
    /// <param name="detail">Detail to be sent with the event, string or other JSON mappable object</param>
    /// <param name="timing">The HTMX request lifecycle step to run the trigger at</param>
    /// <returns></returns>
    public HtmxResponseHeaders WithTrigger(
        string eventName,
        object? detail = null,
        HtmxTriggerTiming timing = HtmxTriggerTiming.Default
    )
    {
        if (!_triggers.ContainsKey(timing))
        {
            _triggers.Add(
                timing,
                new Dictionary<string, object?> { { eventName, detail ?? string.Empty } }
            );
        }
        else
        {
            _triggers[timing].TryAdd(eventName, detail ?? string.Empty);
        }

        return this;
    }

    /// <summary>
    /// Process the Triggers
    /// </summary>
    internal HtmxResponseHeaders Process()
    {
        if (_triggers.ContainsKey(HtmxTriggerTiming.Default))
        {
            ParsePossibleExistingTriggers(Keys.Trigger, HtmxTriggerTiming.Default);

            _headers[Keys.Trigger] = BuildTriggerHeader(HtmxTriggerTiming.Default);
        }

        if (_triggers.ContainsKey(HtmxTriggerTiming.AfterSettle))
        {
            ParsePossibleExistingTriggers(Keys.TriggerAfterSettle, HtmxTriggerTiming.AfterSettle);

            _headers[Keys.TriggerAfterSettle] = BuildTriggerHeader(HtmxTriggerTiming.AfterSettle);
        }

        // ReSharper disable once InvertIf
        if (_triggers.ContainsKey(HtmxTriggerTiming.AfterSwap))
        {
            ParsePossibleExistingTriggers(Keys.TriggerAfterSwap, HtmxTriggerTiming.AfterSwap);

            _headers[Keys.TriggerAfterSwap] = BuildTriggerHeader(HtmxTriggerTiming.AfterSwap);
        }

        return this;
    }

    /// <summary>
    /// Checks to see if the response has an existing header defined by headerKey.
    /// If it does the header loads all of the triggers locally so they aren't overwritten by Htmx.
    /// </summary>
    /// <param name="headerKey"></param>
    /// <param name="timing"></param>
    private void ParsePossibleExistingTriggers(string headerKey, HtmxTriggerTiming timing)
    {
        if (!_headers.ContainsKey(headerKey))
            return;

        var headerValues = _headers.GetValues(headerKey);

        if (headerValues is null)
            return;

        foreach (var headerValue in headerValues)
        {
            if (string.IsNullOrEmpty(headerValue))
                continue;

            // Check if it looks like JSON (starts with { and ends with })
            bool isJson = headerValue.Trim().StartsWith("{") && headerValue.Trim().EndsWith("}");

            if (isJson)
            {
                try
                {
                    var jsonObject = JObject.Parse(headerValue);

                    // Load any existing triggers
                    foreach (var property in jsonObject.Properties())
                    {
                        WithTrigger(property.Name, property.Value.ToString(), timing);
                    }
                }
                catch (JsonReaderException)
                {
                    // If JSON parsing fails, treat as simplified event keys
                    ProcessSimpleTriggers(headerValue, timing);
                }
            }
            else
            {
                ProcessSimpleTriggers(headerValue, timing);
            }
        }

        return;

        void ProcessSimpleTriggers(string? headerValue, HtmxTriggerTiming innerTiming)
        {
            if (headerValue is null)
                return;

            var eventNames = headerValue.Split(',');

            foreach (var eventName in eventNames)
                WithTrigger(eventName.Trim(), null, innerTiming);
        }
    }

    private string BuildTriggerHeader(HtmxTriggerTiming timing)
    {
        // Reduce the payload if the user has only specified 1 trigger with no value
        if (
            _triggers[timing].Count == 1
            && ReferenceEquals(_triggers[timing].First().Value, string.Empty)
        )
        {
            return _triggers[timing].First().Key;
        }

        var jsonHeader = JsonConvert.SerializeObject(_triggers[timing]);
#if DEBUG
        System.Diagnostics.Debug.WriteLine(jsonHeader);
#endif
        return jsonHeader;
    }
}

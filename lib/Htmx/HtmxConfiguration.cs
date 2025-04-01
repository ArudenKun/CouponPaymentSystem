using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Htmx
{
    public static class HtmxConfiguration
    {
        public static HtmxConfig Current { get; } = new HtmxConfig();

        public static void Register(Action<HtmxConfig> configure)
        {
            configure?.Invoke(Current);
        }

        public static IHtmlString GetConfigMetaTag()
        {
            var configJson = GetConfigJson();
            return new HtmlString($"<meta name=\"htmx-config\" content='{configJson}' />");
        }

        private static string GetConfigJson()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Ignore,
            };

            return JsonConvert.SerializeObject(
                new
                {
                    Current.HistoryEnabled,
                    Current.HistoryCacheSize,
                    Current.RefreshOnHistoryMiss,
                    Current.DefaultSwapStyle,
                    Current.DefaultSwapDelay,
                    Current.DefaultSettleDelay,
                    Current.IncludeIndicatorStyles,
                    Current.IndicatorClass,
                    Current.RequestClass,
                    Current.SettlingClass,
                    Current.SwappingClass,
                    Current.AddedClass,
                    Current.SelfRequestsOnly,
                    Current.AllowScriptTags,
                    Current.AllowEval,
                    Current.UseTemplateFragments,
                    Current.WsReconnectDelay,
                    Current.WsBinaryType,
                    Current.DisableSelector,
                    Current.Timeout,
                    Current.InlineScriptNonce,
                    Current.WithCredentials,
                    Current.ScrollBehavior,
                    Current.DefaultFocusScroll,
                    Current.GetCacheBusterParam,
                    Current.GlobalViewTransitions,
                    Current.AttributesToSettle,
                    Current.MethodsThatUseUrlParams,
                    Current.IgnoreTitle,
                    Current.ScrollIntoViewOnBoost,
                    Current.TriggerSpecsCache,
                    AntiForgery = Current.IncludeAntiForgery ? GetAntiForgeryToken() : null,
                },
                settings
            );
        }

        private static object? GetAntiForgeryToken()
        {
            try
            {
                AntiForgery.GetTokens(null, out _, out var formToken);
                return new
                {
                    FormFieldName = "__RequestVerificationToken",
                    HeaderName = "RequestVerificationToken",
                    RequestToken = HttpUtility.HtmlAttributeEncode(formToken),
                };
            }
            catch
            {
                return null;
            }
        }
    }

    public class HtmxConfig
    {
        public bool? HistoryEnabled { get; set; }
        public int? HistoryCacheSize { get; set; } = 10;
        public bool? RefreshOnHistoryMiss { get; set; }
        public string DefaultSwapStyle { get; set; } = "innerHTML";
        public int? DefaultSwapDelay { get; set; }
        public int? DefaultSettleDelay { get; set; } = 100;
        public bool? IncludeIndicatorStyles { get; set; } = true;
        public string IndicatorClass { get; set; } = "htmx-indicator";
        public string RequestClass { get; set; } = "htmx-request";
        public string SettlingClass { get; set; } = "htmx-settling";
        public string SwappingClass { get; set; } = "htmx-swapping";
        public string AddedClass { get; set; } = "htmx-added";
        public bool SelfRequestsOnly { get; set; }
        public bool AllowScriptTags { get; set; } = true;
        public bool? AllowEval { get; set; }
        public bool? UseTemplateFragments { get; set; }
        public string WsReconnectDelay { get; set; } = "full-jitter";
        public string WsBinaryType { get; set; } = "blob";
        public string DisableSelector { get; set; } = "[disable-htmx], [data-disable-htmx]";
        public int? Timeout { get; set; }
        public bool IncludeAntiForgery { get; set; }
        public string InlineScriptNonce { get; set; } = string.Empty;
        public bool WithCredentials { get; set; }
        public string ScrollBehavior { get; set; } = "smooth";
        public bool DefaultFocusScroll { get; set; }
        public bool GetCacheBusterParam { get; set; }
        public bool GlobalViewTransitions { get; set; }

        public IEnumerable<string> AttributesToSettle { get; set; } =
            new List<string> { "class", "style", "width", "height" };

        public IEnumerable<string> MethodsThatUseUrlParams { get; set; } =
            new List<string> { "get" };

        public bool? IgnoreTitle { get; set; }
        public bool? ScrollIntoViewOnBoost { get; set; }

        public HtmxTriggerSpecificationCache TriggerSpecsCache { get; set; } =
            new HtmxTriggerSpecificationCache();
    }

    public class HtmxTriggerSpecificationCache : Dictionary<string, HtmxTriggerSpecification[]>
    {
        // Can add custom methods if needed
    }

    public class HtmxTriggerSpecification
    {
        [JsonProperty("trigger")]
        public string Trigger { get; set; } = string.Empty;

        [JsonProperty("sseEvent")]
        public string? SseEvent { get; set; }

        [JsonProperty("eventFilter")]
        public string? EventFilter { get; set; }

        [JsonProperty("changed")]
        public bool? Changed { get; set; }

        [JsonProperty("once")]
        public bool? Once { get; set; }

        [JsonProperty("consume")]
        public bool? Consume { get; set; }

        [JsonProperty("from")]
        public string? From { get; set; }

        [JsonProperty("target")]
        public string? Target { get; set; }

        [JsonProperty("throttle")]
        public int? Throttle { get; set; }

        [JsonProperty("queue")]
        public string? Queue { get; set; }

        [JsonProperty("root")]
        public string? Root { get; set; }

        [JsonProperty("threshold")]
        public string? Threshold { get; set; }

        [JsonProperty("delay")]
        public int? Delay { get; set; }

        [JsonProperty("pollInterval")]
        public int? PollInterval { get; set; }
    }
}

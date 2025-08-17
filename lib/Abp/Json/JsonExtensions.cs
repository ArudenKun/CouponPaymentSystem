using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using Abp.Json.SystemTextJson;

namespace Abp.Json;

public static class JsonExtensions
{
    private static readonly ConcurrentDictionary<
        object,
        JsonSerializerOptions
    > JsonSerializerOptionsCache;

    static JsonExtensions()
    {
        JsonSerializerOptionsCache = new ConcurrentDictionary<object, JsonSerializerOptions>();
    }

    /// <summary>
    /// Converts given object to JSON string.
    /// </summary>
    /// <returns></returns>
    public static string ToJsonString(
        this object obj,
        bool camelCase = false,
        bool indented = false
    )
    {
        return ToJsonStringWithSystemTextJson(obj, camelCase, indented);
    }

    /// <summary>
    /// Converts given object to JSON string.
    /// </summary>
    /// <returns></returns>
    private static string ToJsonStringWithSystemTextJson(
        this object obj,
        bool camelCase = false,
        bool indented = false
    )
    {
        var options = CreateJsonSerializerOptions(camelCase, indented);
        return ToJsonString(obj, options);
    }

    public static JsonSerializerOptions CreateJsonSerializerOptions(
        bool camelCase = false,
        bool indented = false
    )
    {
        return JsonSerializerOptionsCache.GetOrAdd(
            new { camelCase, indented },
            _ =>
            {
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };

                options.Converters.Add(new AbpStringToEnumFactory());
                options.Converters.Add(new AbpStringToBooleanConverter());
                options.Converters.Add(new AbpStringToGuidConverter());
                options.Converters.Add(new AbpNullableStringToGuidConverter());
                options.Converters.Add(new AbpNullableFromEmptyStringConverterFactory());
                options.Converters.Add(new ObjectToInferredTypesConverter());
                options.Converters.Add(new AbpJsonConverterForType());

                options.TypeInfoResolver = new AbpDateTimeJsonTypeInfoResolver();

                if (camelCase)
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                }

                if (indented)
                {
                    options.WriteIndented = true;
                }

                return options;
            }
        );
    }

    /// <summary>
    /// Converts given object to JSON string using custom <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns></returns>
    public static string ToJsonString(this object? obj, JsonSerializerOptions options)
    {
        return obj != null ? JsonSerializer.Serialize(obj, options) : string.Empty;
    }

    /// <summary>
    /// Returns deserialized string using default <see cref="JsonSerializerOptions"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T FromJsonString<T>(this string value)
    {
        return value.FromJsonString<T>(CreateJsonSerializerOptions());
    }

    /// <summary>
    /// Returns deserialized string using custom <see cref="JsonSerializerOptions"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static T FromJsonString<T>(this string? value, JsonSerializerOptions options)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return value != null ? JsonSerializer.Deserialize<T>(value, options) : default;
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <summary>
    /// Returns deserialized string using explicit <see cref="Type"/> and custom <see cref="JsonSerializerOptions"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object? FromJsonString(
        this string? value,
        Type type,
        JsonSerializerOptions options
    )
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return value != null ? JsonSerializer.Deserialize(value, type, options) : null;
    }
}

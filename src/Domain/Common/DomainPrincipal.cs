using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Domain.Common;

public class DomainPrincipal : ClaimsPrincipal
{
    private static readonly Regex InvalidEnumValueChars = new(
        "[^a-zA-z0-9_]",
        RegexOptions.Compiled
    );

    public DomainPrincipal(IPrincipal principal)
        : base(principal) { }

    public T? GetClaimValue<T>(string claimType, T? defaultValue = default)
    {
        var claim = FindFirst(claimType)?.Value;
        return claim is not null ? ChangeType(claim, defaultValue) : defaultValue;
    }

    private static T? ChangeType<T>(object? source, T? defaultValue = default)
    {
        try
        {
            switch (source)
            {
                case null:
                    return defaultValue;
                case T value:
                    return value;
            }

            var conversionType = typeof(T);

            // Nullable
            if (
                conversionType.IsGenericType
                && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                conversionType = new NullableConverter(conversionType).UnderlyingType;
            }

            if (conversionType.IsEnum)
            {
                var enumValue = source.ToString();
                if (!string.IsNullOrEmpty(enumValue))
                {
                    enumValue = InvalidEnumValueChars.Replace(enumValue, string.Empty);
                }

                return (T)Enum.Parse(conversionType, enumValue, true);
            }

            // String
            if (conversionType == typeof(string))
            {
                return (T)(object)Convert.ToString(source);
            }

            // Guid
            if (conversionType == typeof(Guid))
            {
                var guidValue = source.ToString();

                if (string.IsNullOrEmpty(guidValue))
                {
                    return defaultValue;
                }

                if (Guid.TryParse(guidValue, out var result))
                {
                    return (T)(object)result;
                }
            }

            // Bool
            if (conversionType == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(source);
            }

            // DateTime
            if (conversionType == typeof(DateTime))
            {
                return (T)(object)DateTime.Parse(source.ToString(), CultureInfo.CurrentCulture);
            }

            // TimeSpan
            if (conversionType == typeof(TimeSpan))
            {
                return (T)(object)TimeSpan.Parse(source.ToString(), CultureInfo.CurrentCulture);
            }

            // Default
            return (T)Convert.ChangeType(source, conversionType);
        }
        catch
        {
            return defaultValue;
        }
    }
}

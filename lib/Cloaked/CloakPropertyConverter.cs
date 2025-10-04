using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cloaked;

/// <summary>
/// JSON converter for properties marked with [Cloak].
/// </summary>
public class CloakPropertyConverter : JsonConverter<object>
{
    private readonly Type _propertyType;
    private readonly ICloak _cloak;

    /// <summary>
    /// JSON converter for properties marked with [Cloak].
    /// </summary>
    public CloakPropertyConverter(Type propertyType, ICloak cloak)
    {
        _propertyType = propertyType;
        _cloak = cloak;
    }

    /// <summary>
    /// Reads and converts the JSON to the specified type.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>The converted value.</returns>
    public override object? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            if (Nullable.GetUnderlyingType(_propertyType) != null)
                return null;
            throw new JsonException($"Cannot convert null to non-nullable type {_propertyType}");
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var encodedValue = reader.GetString()!;
            return _cloak.Decode(encodedValue, _propertyType);
        }

        throw new JsonException(
            $"Expected string token for Cloak property of type {_propertyType}, but got {reader.TokenType}"
        );
    }

    /// <summary>
    /// Writes the specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var encoded = _cloak.Encode(value, _propertyType);
        writer.WriteStringValue(encoded);
    }
}

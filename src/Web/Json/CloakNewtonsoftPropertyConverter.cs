using Cloaked;
using Newtonsoft.Json;

namespace Web.Json;

public class CloakNewtonsoftPropertyConverter : JsonConverter
{
    private readonly Type _propertyType;
    private readonly ICloak _codec;

    public CloakNewtonsoftPropertyConverter(Type propertyType, ICloak codec)
    {
        _propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        _codec = codec;
    }

    public override bool CanConvert(Type objectType)
    {
        // Allow handling both the propertyType and Nullable<propertyType>
        var actualType = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return actualType == _propertyType;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var encoded = _codec.Encode(value, _propertyType);
        writer.WriteValue(encoded);
    }

    public override object? ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer
    )
    {
        if (reader.TokenType == JsonToken.Null)
        {
            if (Nullable.GetUnderlyingType(objectType) != null)
                return null;

            throw new JsonSerializationException(
                $"Cannot convert null to non-nullable type {_propertyType}"
            );
        }

        if (reader.TokenType == JsonToken.String)
        {
            var encodedValue = (string)reader.Value!;
            return _codec.Decode(encodedValue, _propertyType);
        }

        throw new JsonSerializationException(
            $"Expected string token for CloakId property of type {_propertyType}, but got {reader.TokenType}"
        );
    }
}

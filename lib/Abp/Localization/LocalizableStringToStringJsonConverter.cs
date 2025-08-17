using System.Text.Json;
using System.Text.Json.Serialization;

namespace Abp.Localization;

/// <summary>
/// This class can be used to serialize <see cref="ILocalizableString"/> to <see cref="string"/> during serialization.
/// It does not work for deserialization.
/// </summary>
public class LocalizableStringToStringJsonConverter : JsonConverter<ILocalizableString>
{
    public override ILocalizableString Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        // Deserialization not supported
        throw new NotImplementedException();
    }

    public override void Write(
        Utf8JsonWriter writer,
        ILocalizableString value,
        JsonSerializerOptions options
    )
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(
            value.Localize(new LocalizationContext(LocalizationHelper.Manager))
        );
    }
}

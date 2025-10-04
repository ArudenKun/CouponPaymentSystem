using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web.Utilities;

public class SystemTextJsonMediaTypeFormatter : MediaTypeFormatter
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonMediaTypeFormatter(JsonSerializerOptions? options = null)
    {
        SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        SupportedEncodings.Add(new UTF8Encoding(false, true));
        SupportedEncodings.Add(Encoding.Unicode);

        _options =
            options
            ?? new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
            };
    }

    public override bool CanReadType(Type type) => true;

    public override bool CanWriteType(Type type) => true;

    public override async Task<object?> ReadFromStreamAsync(
        Type type,
        Stream readStream,
        HttpContent content,
        IFormatterLogger formatterLogger
    ) => await JsonSerializer.DeserializeAsync(readStream, type, _options);

    public override async Task WriteToStreamAsync(
        Type type,
        object? value,
        Stream writeStream,
        HttpContent content,
        TransportContext transportContext
    ) => await JsonSerializer.SerializeAsync(writeStream, value, type, _options);
}

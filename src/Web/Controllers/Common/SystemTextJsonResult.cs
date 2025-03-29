using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web.Mvc;
using Riok.Mapperly.Abstractions;

namespace Web.Controllers.Common;

public class SystemTextJsonResult : JsonResult
{
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    public override void ExecuteResult(ControllerContext context)
    {
        Guard.NotNull(context);

        if (
            JsonRequestBehavior is JsonRequestBehavior.DenyGet
            && string.Equals(
                context.HttpContext.Request.HttpMethod,
                "GET",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            throw new InvalidOperationException(
                "JsonNetResult cannot be executed in a GET request"
            );
        }

        var response = context.HttpContext.Response;
        response.ContentType = !string.IsNullOrEmpty(ContentType)
            ? ContentType
            : "application/json";

        if (ContentEncoding != null)
            response.ContentEncoding = ContentEncoding;

        if (Data is null)
            return;

        JsonSerializer.Serialize(response.OutputStream, Data, DefaultSerializerOptions);
    }
}

[Mapper]
public static partial class JsonNetResultMapper
{
    public static partial SystemTextJsonResult ToSystemTextJsonResult(this JsonResult jsonResult);
}

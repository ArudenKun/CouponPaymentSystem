using System.Web.Mvc;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Web.Controllers.Common;

public class JsonNetResult : JsonResult
{
    private static readonly JsonSerializerSettings DefaultSerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
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

        var serializer = JsonSerializer.Create(DefaultSerializerSettings);
        serializer.Serialize(response.Output, Data);
    }
}

[Mapper]
public static partial class JsonNetResultMapper
{
    public static partial JsonNetResult ToJsonNetResult(this JsonResult jsonResult);
}

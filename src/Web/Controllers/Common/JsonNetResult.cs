using System.Web.Mvc;
using Ardalis.GuardClauses;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Web.Controllers.Common;

public class JsonNetResult : JsonResult
{
    private static readonly JsonSerializerSettings DefaultSerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
    };

    public override void ExecuteResult(ControllerContext context)
    {
        Guard.Against.Null(context);

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

        var jsonSerializer = JsonSerializer.Create(DefaultSerializerSettings);
        jsonSerializer.Serialize(response.Output, Data);
    }
}

[Mapper]
public static partial class JsonNetResultMapper
{
    public static partial JsonNetResult ToJsonNetResult(this JsonResult jsonResult);
}

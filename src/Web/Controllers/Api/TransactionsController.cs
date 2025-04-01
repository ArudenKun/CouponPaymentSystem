using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.Features.Transactions.Commands;
using Application.Features.Transactions.Queries;
using Application.Features.Users;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using Htmx;
using MediatR;
using Web.Controllers.Common;

namespace Web.Controllers.Api;

[RoutePrefix("api/transactions")]
public class TransactionsController : AppControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("table")]
    public async Task<ActionResult> Table(IDataTablesRequest request)
    {
        var response = await _mediator.Send(new GetTransactionsDataTableQuery(request));
        var dtResponse = DataTablesResponse.Create(
            request,
            (int)response.Value.RecordsTotal,
            (int)response.Value.RecordsFiltered,
            response.Value.Data
        );

        Response.Htmx(h => h.WithTrigger("reloadTable"));
        return new DataTablesJsonResult(dtResponse, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    [Route]
    public ActionResult GetAll()
    {
        return JsonGet(User.MapToUser());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> Get(string id)
    {
        await Task.CompletedTask;
        return JsonGet(id);
        // var response = await _mediator.Send(new GetTransactionQuery(id));
        // return response.Match(JsonGet, JsonGet);
    }

    [HttpPost]
    [Route]
    public async Task<ActionResult> Post(HttpPostedFileBase file, string currency)
    {
        var response = await _mediator.Send(new UploadTransactionsCommand(file, currency));

        if (!response.IsError)
            return new HttpStatusCodeResult(HttpStatusCode.Created);

        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return JsonGet(response.Errors);
    }

    [HttpPut]
    [HttpPatch]
    [Route("{id:int}")]
    public ActionResult Update(int id)
    {
        return JsonGet(new { Id = id, UserTest = User.MapToUser() });
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult Delete(int id)
    {
        return JsonGet($"Delete {id}");
    }
}

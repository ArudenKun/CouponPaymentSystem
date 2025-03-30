using System.Web.Mvc;
using Application.Features.Transactions.Queries;
using Application.Features.Users;
using Domain.Entities;
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
    [Route]
    public ActionResult GetAll()
    {
        return JsonGet(User.MapToUser());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> Get(string id)
    {
        var response = await _mediator.Send(new GetTransactionQuery(id));
        return response.Match(JsonGet, JsonGet);
    }

    [HttpPost]
    [Route]
    public ActionResult Post(IEnumerable<Transaction> transactions)
    {
        return JsonGet(new { Transactions = transactions, User = User.MapToUser() });
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

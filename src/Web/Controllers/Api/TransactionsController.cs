using System.Web.Mvc;
using Domain.Entities;
using Web.Controllers.Common;

namespace Web.Controllers.Api;

[RoutePrefix("api/transactions")]
[AllowAnonymous]
public class TransactionsController : AppControllerBase
{
    [HttpGet]
    [Route]
    public ActionResult GetAll(int page = 1, int pageSize = 10)
    {
        if (page < 1)
            page = 1;
        if (pageSize < 1 || pageSize > 100)
            pageSize = 10;

        // Your logic here
        return JsonGet(new { page, pageSize });
    }

    [HttpGet]
    [Route("{id:int}")]
    public ActionResult Get(int id)
    {
        return JsonGet($"Get {id}");
    }

    [HttpPost]
    [Route]
    public ActionResult Post(IEnumerable<Transaction> transactions)
    {
        return Json(transactions);
    }

    [HttpPut]
    [HttpPatch]
    [Route("{id:int}")]
    public ActionResult Update()
    {
        return JsonGet(User);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult Delete(int id)
    {
        return JsonGet($"Delete {id}");
    }
}

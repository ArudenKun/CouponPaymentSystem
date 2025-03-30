using Application.Common.Interfaces.Caching;
using DataTables.AspNet.Core;

namespace Application.Features.Transactions.Queries;

public class GetTransactionsDataTableQuery : ICacheableRequest<TransactionDataTableResponse>
{
    public GetTransactionsDataTableQuery(IDataTablesRequest request)
    {
        Request = request;
    }

    public IDataTablesRequest Request { get; }

    public string CacheKey =>
        $"{nameof(GetTransactionsDataTableQuery)}-"
        + $"Draw:{Request.Draw}-"
        + $"Start:{Request.Start}-"
        + $"Length:{Request.Length}-"
        + $"Order:{Request.Columns.FirstOrDefault()?.Sort.Order}-"
        + $"OrderDirection:{Request.Columns.FirstOrDefault()?.Sort.Direction}-"
        + $"Search:{Request.Search.Value}-"
        + $"PageSize:{Request.Columns.FirstOrDefault()?.Name}-"
        + $"Skip:{Request.Start}";
    public IEnumerable<string>? Tags => null;
}

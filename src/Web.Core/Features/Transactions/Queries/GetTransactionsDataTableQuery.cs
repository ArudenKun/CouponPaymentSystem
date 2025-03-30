using DataTables.AspNet.Core;
using FluentValidation;

namespace Web.Core.Features.Transactions.Queries;

public sealed class GetTransactionsDataTableQuery : ICacheableRequest<TransactionDataTableResponse>
{
    public GetTransactionsDataTableQuery(IDataTablesRequest request, string userId, bool isMatch)
    {
        Draw = request.Draw;
        Start = request.Start;
        Length = request.Length;
        Search = request.Search;
        Columns = request.Columns;
        Order = request.Order;
        OrderDirection = request.OrderDirection;
        UserId = userId;
        IsMatch = isMatch;
        CacheKey = $"{nameof(GetTransactionsDataTableQuery)}-";
    }

    public int Draw { get; }

    public int Start { get; }

    public int Length { get; }

    public ISearch Search { get; }

    public IEnumerable<IColumn> Columns { get; }
    public int Order { get; }
    public SortDirection OrderDirection { get; }
    public string UserId { get; }
    public bool IsMatch { get; }

    public string CacheKey { get; }
    public IEnumerable<string>? Tags => null;
}

public class GetTransactionsDataTableQueryValidator
    : AbstractValidator<GetTransactionsDataTableQuery>
{
    public GetTransactionsDataTableQueryValidator() { }
}

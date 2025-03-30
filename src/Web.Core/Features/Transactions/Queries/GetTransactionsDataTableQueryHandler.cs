using System.Data;
using DataTables.AspNet.Core;
using Dommel;
using ErrorOr;
using Web.Core.Models;

namespace Web.Core.Features.Transactions.Queries;

public class GetTransactionsDataTableQueryHandler
    : IAppRequestHandler<GetTransactionsDataTableQuery, TransactionDataTableResponse>
{
    private readonly IDbConnection _dbConnection;

    public GetTransactionsDataTableQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ErrorOr<TransactionDataTableResponse>> Handle(
        GetTransactionsDataTableQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactionsCount = await _dbConnection.CountAsync<Transaction>();
        var transactions = (
            await _dbConnection.FromAsync<Transaction>(sql =>
            {
                sql = sql.Where(p => p.IsMatch == request.IsMatch && p.UserId == request.UserId);
                if (!string.IsNullOrEmpty(request.Search.Value))
                {
                    sql = sql.Where(p => p.AccountNumber.Contains(request.Search.Value))
                        .OrWhere(p => p.AccountName.Contains(request.Search.Value))
                        .OrWhere(p => p.CasaAccountName.Contains(request.Search.Value))
                        .OrWhere(p => p.NewAccountNumber.Contains(request.Search.Value));
                }

                var orderColumn = request.Columns.FirstOrDefault(c =>
                    c.Sort.Order == request.Order
                );
                var sortProperty = Resolvers
                    .Properties(typeof(Transaction))
                    .FirstOrDefault(c =>
                        string.Equals(
                            c.Property.Name,
                            orderColumn?.Name,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    ?.Property;

                if (sortProperty is not null)
                {
                    sql =
                        request.OrderDirection is SortDirection.Ascending
                            ? sql.OrderBy(sortProperty)
                            : sql.OrderByDescending(sortProperty);
                }

                sql.Select();
            })
        ).ToList();

        return new TransactionDataTableResponse
        {
            Data = transactions,
            TotalRecords = (int)transactionsCount,
            TotalRecordsFiltered = transactions.Count,
        };
    }
}

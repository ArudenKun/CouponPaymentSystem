using Application.Common.Interfaces;
using ErrorOr;

namespace Application.Features.Transactions.Queries;

public sealed class GetTransactionsDataTableQueryHandler
    : IAppRequestHandler<GetTransactionsDataTableQuery, TransactionDataTableResponse>
{
    private readonly IAppDbContext _context;

    public GetTransactionsDataTableQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<TransactionDataTableResponse>> Handle(
        GetTransactionsDataTableQuery request,
        CancellationToken cancellationToken
    )
    {
        // var a = await _context.Transactions.SelectAsync(x => x.AccountName == "");
        throw new NotImplementedException();
    }
}

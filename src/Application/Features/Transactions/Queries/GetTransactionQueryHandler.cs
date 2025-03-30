using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Transactions.Queries;

public class GetTransactionQueryHandler
    : IRequestHandler<GetTransactionQuery, ErrorOr<TransactionDto>>
{
    public Task<ErrorOr<TransactionDto>> Handle(
        GetTransactionQuery request,
        CancellationToken cancellationToken
    )
    {
        var transaction = new Transaction
        {
            Id = Guid.Parse(request.TransactionId),
            AccountName = GuidPolyfill.CreateVersion7().ToString(),
            AccountNumber = GuidPolyfill.CreateVersion7().ToString(),
        };

        return Task.FromResult(transaction.MapToDto().ToErrorOr());
    }
}

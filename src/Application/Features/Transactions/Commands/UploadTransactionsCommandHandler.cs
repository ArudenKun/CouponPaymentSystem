using Application.Common.Interfaces;
using ErrorOr;

namespace Application.Features.Transactions.Commands;

public class UploadTransactionsCommandHandler
    : IAppRequestHandler<UploadTransactionsCommand, TransactionDataTableResponse>
{
    public Task<ErrorOr<TransactionDataTableResponse>> Handle(
        UploadTransactionsCommand request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

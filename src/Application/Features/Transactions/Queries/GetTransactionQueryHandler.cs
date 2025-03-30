// using Application.Common.Interfaces;
// using Domain.Entities;
// using ErrorOr;
//
// namespace Application.Features.Transactions.Queries;
//
// public class GetTransactionQueryHandler
//     : IAppRequestHandler<GetTransactionQuery, TransactionDataTableResponse>
// {
//     public Task<ErrorOr<TransactionDataTableResponse>> Handle(
//         GetTransactionQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         var transaction = new Transaction
//         {
//             Id = Guid.Parse(request.TransactionId),
//             AccountName = GuidPolyfill.CreateVersion7().ToString(),
//             AccountNumber = GuidPolyfill.CreateVersion7().ToString(),
//         };
//
//         return Task.FromResult(transaction.MapToResponseDto().ToErrorOr());
//     }
// }

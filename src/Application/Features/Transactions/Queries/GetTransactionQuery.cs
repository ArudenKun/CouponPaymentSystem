using Application.Common.Interfaces.Caching;
using ErrorOr;
using FluentValidation;

namespace Application.Features.Transactions.Queries;

public class GetTransactionQuery : ICacheableRequest<ErrorOr<TransactionDto>>
{
    public GetTransactionQuery(string transactionId)
    {
        TransactionId = transactionId;
    }

    public string TransactionId { get; }
    public string CacheKey => $"{nameof(GetTransactionQuery)}-{TransactionId}";
    public IEnumerable<string>? Tags => null;
}

public class GetTransactionQueryValidator : AbstractValidator<GetTransactionQuery>
{
    public GetTransactionQueryValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("Transaction Id is required")
            .Must(id => id.Length is 32 or 36)
            .WithMessage("Transaction Id must be 32 or 36 characters long")
            .Must(id =>
                Guid.TryParseExact(id, "D", out _)
                || // With hyphens (36 chars)
                Guid.TryParseExact(id, "N", out _)
            ) // Without hyphens (32 chars)
            .WithMessage("Transaction Id must be a valid format");
    }
}

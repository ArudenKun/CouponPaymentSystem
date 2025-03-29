using Domain.Entities;
using Domain.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Transactions;

public class TransactionDto
{
    public required TransactionId Id { get; init; }
    public required AccountNumber AccountNumber { get; init; }
    public required AccountName AccountName { get; init; }

    public int Age { get; init; }
}

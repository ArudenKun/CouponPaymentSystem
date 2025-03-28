using Domain.Common.Entities;
using Domain.ValueObjects;
using Vogen;

namespace Domain.Entities;

[ValueObject<Guid>]
public readonly partial struct TransactionId;

public class Transaction : IEntity<TransactionId>
{
    public required TransactionId Id { get; init; }
    public AccountName AccountName { get; set; }
    public AccountNumber AccountNumber { get; set; }
}

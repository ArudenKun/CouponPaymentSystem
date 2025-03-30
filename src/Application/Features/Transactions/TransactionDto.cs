namespace Application.Features.Transactions;

public class TransactionDto
{
    public required Guid Id { get; init; }
    public required string AccountNumber { get; init; }
    public required string AccountName { get; init; }
}

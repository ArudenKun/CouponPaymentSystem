namespace Application.Features.Transactions;

public class TransactionDto
{
    public required Guid Id { get; init; }
    public required string AccountNumber { get; set; } = string.Empty;
    public required string AccountName { get; set; } = string.Empty;
    public required string AccountType { get; set; } = string.Empty;
}

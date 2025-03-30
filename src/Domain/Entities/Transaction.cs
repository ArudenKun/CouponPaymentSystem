using Domain.Common.Entities;

namespace Domain.Entities;

public class Transaction : IEntity
{
    public required Guid Id { get; init; }

    public string AccountName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;
}

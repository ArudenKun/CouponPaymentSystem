using System.ComponentModel.DataAnnotations;
using Domain.Common.Entities;

namespace Domain.Entities;

public class Transaction : IEntity
{
    [Key]
    public required Guid Id { get; init; }

    public required string AccountNumber { get; set; } = string.Empty;
    public required string AccountName { get; set; } = string.Empty;
    public required string AccountType { get; set; } = string.Empty;
}

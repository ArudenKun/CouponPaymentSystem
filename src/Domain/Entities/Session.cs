using Domain.Common.Entities;

namespace Domain.Entities;

public class Session : IEntity
{
    public required int Id { get; init; }
    public required int Code { get; init; }
    public string? Message { get; init; }
}

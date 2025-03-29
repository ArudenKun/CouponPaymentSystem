using Domain.Common.Entities;

namespace Domain.Entities;

public readonly partial struct SessionId;

public class Session : IEntity<SessionId>
{
    public required SessionId Id { get; init; }
    public required int Code { get; init; }
    public string? Message { get; init; }
}

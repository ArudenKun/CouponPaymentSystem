using Domain.Common.Entities;
using Vogen;

namespace Domain.Entities;

[ValueObject<string>]
public readonly partial struct UserId;

public class User : IEntity<UserId>
{
    private const string EmptyValue = "UNKNOWN";

    public static readonly User Empty = new()
    {
        Id = UserId.From(EmptyValue),
        SessionId = SessionId.From(0),
        FirstName = EmptyValue,
        MiddleName = EmptyValue,
        LastName = EmptyValue,
        AppRoleId = EmptyValue,
        AppRoleName = EmptyValue,
    };

    public required UserId Id { get; init; }
    public required SessionId SessionId { get; init; }
    public required string FirstName { get; init; }
    public required string MiddleName { get; init; }
    public required string LastName { get; init; }
    public required string AppRoleId { get; init; }
    public required string AppRoleName { get; init; }
}

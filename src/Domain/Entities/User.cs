using Domain.Common.Entities;

namespace Domain.Entities;

public class User : IEntity
{
    private const string EmptyValue = "UNKNOWN";

    public static readonly User Empty = new()
    {
        Id = EmptyValue,
        FirstName = EmptyValue,
        MiddleName = EmptyValue,
        LastName = EmptyValue,
        AppRoleId = EmptyValue,
        AppRoleName = EmptyValue,
    };

    public required string Id { get; init; }
    public required string FirstName { get; init; }
    public required string MiddleName { get; init; }
    public required string LastName { get; init; }
    public required string AppRoleId { get; init; }
    public required string AppRoleName { get; init; }

    public string FullName => $"{FirstName} {MiddleName} {LastName}";
}

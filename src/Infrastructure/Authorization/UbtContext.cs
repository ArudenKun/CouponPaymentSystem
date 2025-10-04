namespace Infrastructure.Authorization;

internal record UbtContext(
    string DomainId,
    string FirstName,
    string LastName,
    string RoleId,
    string RoleName,
    string DivisionId,
    string DivisionName
);

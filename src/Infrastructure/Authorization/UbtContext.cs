namespace Infrastructure.Authorization;

internal record UbtContext(
    string DomainId,
    string FirstName,
    string LastName,
    int RoleId,
    string RoleName,
    int DivisionId,
    string DivisionName
);

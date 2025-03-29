using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using Domain;
using Domain.Entities;

namespace Application;

public class AppPrincipal : DomainPrincipal
{
    private readonly User _user;

    public AppPrincipal(IPrincipal principal)
        : base(principal)
    {
        _user =
            JsonSerializer.Deserialize<User?>(GetClaimValue<string>(ClaimTypes.UserData) ?? "")
            ?? User.Empty;
    }

    public SessionId SessionId => _user.SessionId;
    public UserId UserId => _user.Id;
    public string FirstName => _user.FirstName;
    public string MiddleName => _user.MiddleName;
    public string LastName => _user.LastName;
    public string AppRoleId => _user.AppRoleId;
    public string AppRoleName => _user.AppRoleName;
}

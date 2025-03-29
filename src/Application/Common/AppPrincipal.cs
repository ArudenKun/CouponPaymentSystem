using System.Security.Claims;
using System.Security.Principal;
using Ardalis.GuardClauses;
using Domain;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Common;

public class AppPrincipal : DomainPrincipal
{
    private User? _user;

    private User User
    {
        get
        {
            if (_user is not null)
            {
                return _user;
            }

            var userDataClaim = FindFirst(ClaimTypes.UserData)?.Value;

            if (string.IsNullOrEmpty(userDataClaim))
            {
                return _user = User.Empty;
            }

            Guard.Against.NullOrEmpty(userDataClaim);
            return _user = JsonConvert.DeserializeObject<User>(userDataClaim) ?? User.Empty;
        }
    }

    public AppPrincipal(IPrincipal principal)
        : base(principal) { }

    public SessionId SessionId => User.SessionId;
    public UserId UserId => User.Id;
    public string FirstName => User.FirstName;
    public string MiddleName => User.MiddleName;
    public string LastName => User.LastName;
    public string AppRoleId => User.AppRoleId;
    public string AppRoleName => User.AppRoleName;
}

using System.Security.Claims;
using System.Security.Principal;
using Domain;
using Domain.Common;
using Domain.Common.Enums;
using Domain.Entities;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Application.Common;

public class AppPrincipal : DomainPrincipal
{
    private readonly Lazy<User> _user;
    private readonly Lazy<int> _sessionId;

    public AppPrincipal(IPrincipal principal)
        : base(principal)
    {
        _user = new Lazy<User>(InitializeUser);
        _sessionId = new Lazy<int>(InitializeSessionId);
    }

    private User InitializeUser()
    {
        var userDataClaim = FindFirst(ClaimTypes.UserData);
        if (userDataClaim is not null)
        {
            return JsonConvert.DeserializeObject<User>(userDataClaim.Value) ?? User.Empty;
        }

        return User.Empty;
    }

    private int InitializeSessionId()
    {
        var sessionIdClaim = FindFirst(DomainClaimTypes.SessionId)?.Value;
        return int.TryParse(sessionIdClaim, out var sessionId) ? sessionId : 0;
    }

    public int SessionId => _sessionId.Value;
    public string Id => _user.Value.Id;
    public string FirstName => _user.Value.FirstName;
    public string MiddleName => _user.Value.MiddleName;
    public string LastName => _user.Value.LastName;
    public string AppRoleId => _user.Value.AppRoleId;
    public string AppRoleName => _user.Value.AppRoleName;
}

using System.Security.Claims;
using System.Security.Principal;

namespace Web.Core;

public sealed class AppPrincipal : ClaimsPrincipal
{
    public AppPrincipal(IPrincipal principal)
        : base(principal) { }
}

using System.ComponentModel;
using System.Security;
using System.Web;

namespace Castle.Facilities.AspNet.SystemWeb;

[SecurityCritical]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class PerWebRequestLifestyleModuleRegistration
{
    public static void Run()
    {
        HttpApplication.RegisterModule(typeof(PerWebRequestLifestyleModule));
    }
}

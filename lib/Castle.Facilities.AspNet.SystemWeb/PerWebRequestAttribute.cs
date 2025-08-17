using Castle.Core;

namespace Castle.Facilities.AspNet.SystemWeb;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class PerWebRequestAttribute : ScopedAttribute
{
    public PerWebRequestAttribute()
    {
        ScopeAccessorType = typeof(WebRequestScopeAccessor);
    }
}

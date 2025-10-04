using Cps.Core.Common;

namespace Domain.Common;

public static class EnvironmentHelper
{
    public static string EnvironmentName => DebugHelper.IsDebug ? "Development" : "Production";
}

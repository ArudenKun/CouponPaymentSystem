namespace CouponPaymentSystem.Infrastructure.Helpers;

public static class DebugHelper
{
    public static bool IsDebug
#if DEBUG
        => true;
#else
        => false;
#endif
}

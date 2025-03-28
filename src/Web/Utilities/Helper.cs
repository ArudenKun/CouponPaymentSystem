namespace Web.Utilities;

public static class Helper
{
    public static bool IsDebug
#if DEBUG
        => true;
#else
        => false;
#endif
}

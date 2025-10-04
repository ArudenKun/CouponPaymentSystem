namespace Domain;

public static class CpsConstants
{
    public const string Name = "CouponPaymentSystem";
    public const string DisplayName = "Coupon Payment System";
    public const string ShortName = "Cps";
    public const bool MultiTenancyEnabled = true;

    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public const string DefaultPassPhrase = "{{65753864-3033-44D9-B65B-13C68C2A2372}}";
}

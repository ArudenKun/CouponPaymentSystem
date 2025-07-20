using Vogen;

namespace CouponPaymentSystem.Core.Configuration;

[ValueObject<string>]
public partial record SystemId
{
    private static string NormalizeInput(string input) => input.PadLeft(3, '0');
}

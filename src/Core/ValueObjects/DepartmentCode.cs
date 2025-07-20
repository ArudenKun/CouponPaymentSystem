using CouponPaymentSystem.Core.Common.Extensions;
using Vogen;

namespace CouponPaymentSystem.Core.ValueObjects;

[ValueObject<string>]
public partial record DepartmentCode
{
    private static string NormalizeInput(string input) => input.PadLeftZero(4);
}

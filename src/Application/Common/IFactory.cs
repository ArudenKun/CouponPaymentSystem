namespace CouponPaymentSystem.Application.Common;

public interface IFactory<out T>
{
    T Create();
}

using System.Data.Common;
using Ardalis.SmartEnum;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace CouponPaymentSystem.Core.Common;

public class SmartEnumValueUserType<TEnum> : SmartEnumValueUserType<TEnum, int>
    where TEnum : SmartEnum<TEnum, int>;

public class SmartEnumValueUserType<TEnum, TValue> : UserTypeBase
    where TEnum : SmartEnum<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    public override SqlType[] SqlTypes => [GetSqlType(typeof(TValue))];
    public override Type ReturnedType => typeof(TEnum);
    public override bool IsMutable => false;

    public override object NullSafeGet(
        DbDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner
    ) =>
        SmartEnum<TEnum, TValue>.FromValue(
            (TValue)
                NHibernateUtil.GuessType(typeof(TValue)).NullSafeGet(rs, names[0], session, owner)
        );

    public override void NullSafeSet(
        DbCommand cmd,
        object value,
        int index,
        ISessionImplementor session
    ) =>
        NHibernateUtil
            .GuessType(typeof(TValue))
            .NullSafeSet(cmd, ((TEnum)value).Value, index, session);
}

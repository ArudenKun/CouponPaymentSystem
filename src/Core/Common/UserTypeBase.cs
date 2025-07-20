using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace CouponPaymentSystem.Core.Common;

public abstract class UserTypeBase : IUserType
{
    public abstract SqlType[] SqlTypes { get; }
    public abstract Type ReturnedType { get; }
    public abstract bool IsMutable { get; }

    public new virtual bool Equals(object x, object y) => ReferenceEquals(x, y);

    public virtual int GetHashCode(object x) => x.GetHashCode();

    public abstract object NullSafeGet(
        DbDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner
    );

    public abstract void NullSafeSet(
        DbCommand cmd,
        object value,
        int index,
        ISessionImplementor session
    );

    public virtual object DeepCopy(object value) => value;

    public virtual object Replace(object original, object target, object owner) => original;

    public virtual object Assemble(object cached, object owner) => cached;

    public virtual object Disassemble(object value) => value;

    protected virtual SqlType GetSqlType(Type type)
    {
        if (type == typeof(short))
            return NHibernateUtil.Int16.SqlType;
        if (type == typeof(int))
            return NHibernateUtil.Int32.SqlType;
        if (type == typeof(long))
            return NHibernateUtil.Int64.SqlType;
        if (type == typeof(decimal))
            return NHibernateUtil.Decimal.SqlType;
        if (type == typeof(double))
            return NHibernateUtil.Double.SqlType;
        if (type == typeof(string))
            return NHibernateUtil.String.SqlType;
        if (type == typeof(DateTime))
            return NHibernateUtil.DateTime.SqlType;
        if (type == typeof(bool))
            return NHibernateUtil.Boolean.SqlType;
        if (type == typeof(byte))
            return NHibernateUtil.Byte.SqlType;

        throw new NotImplementedException("Type not yet implemented");
    }
}

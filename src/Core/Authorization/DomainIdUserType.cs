using System.Data.Common;
using CouponPaymentSystem.Core.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace CouponPaymentSystem.Core.Authorization;

public class DomainIdUserType : UserTypeBase
{
    public override SqlType[] SqlTypes => [NHibernateUtil.String.SqlType];
    public override Type ReturnedType => typeof(DomainId);
    public override bool IsMutable => false;

    public override object NullSafeGet(
        DbDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner
    ) => DomainId.From((string)NHibernateUtil.String.NullSafeGet(rs, names[0], session, owner));

    public override void NullSafeSet(
        DbCommand cmd,
        object value,
        int index,
        ISessionImplementor session
    ) => NHibernateUtil.String.NullSafeSet(cmd, (string)value, index, session);
}

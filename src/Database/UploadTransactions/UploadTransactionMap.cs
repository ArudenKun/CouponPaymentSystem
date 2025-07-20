using Abp.NHibernate.EntityMappings;
using CouponPaymentSystem.Core.UploadTransactions;
using CouponPaymentSystem.Database.Common;

namespace CouponPaymentSystem.Database.UploadTransactions;

public class UploadTransactionMap : EntityMap<UploadTransaction>
{
    public UploadTransactionMap()
        : base(nameof(UploadTransaction))
    {
        Id(x => x.Id).GeneratedBy.Identity();
        Map(x => x.AccountNumber).Not.Nullable().Length(16);
        Map(x => x.NewAccountNumber).Not.Nullable().Length(16);
        Map(x => x.AccountName).Not.Nullable();
        Map(x => x.CasaAccountName).Not.Nullable();
        Map(x => x.AccountType).Nullable().Length(2);
        Map(x => x.TransactionAmount).Not.Nullable().Precision(18).Scale(2);
        Map(x => x.NetAmount).Not.Nullable().Precision(18).Scale(2);
        Map(x => x.IsMatch).Not.Nullable().Index("IDX_UploadTransaction_IsMatch");
        Map(x => x.IsRejected).Not.Nullable().Index("IDX_UploadTransaction_IsRejected");
        Map(x => x.Reason)
            .Not.Nullable()
            .CustomType<SmartEnumValueUserType<UploadTransactionReason>>();
        References(x => x.Upload).Column("UploadId");
    }
}

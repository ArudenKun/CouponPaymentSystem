using Abp.NHibernate.EntityMappings;

namespace CouponPaymentSystem.Core.GrsAlliedAccounts;

public class GrsAlliedAccountMap : EntityMap<GrsAlliedAccount, string>
{
    public GrsAlliedAccountMap()
        : base("tbl_grs_allied_acct")
    {
        Id(x => x.Id).Column("acct_number").Not.Nullable().Length(16);
        Map(x => x.AccountName1).Column("acct_name1").Not.Nullable().Length(40);
        Map(x => x.AccountName2).Column("acct_name2").Nullable().Length(40);
        Map(x => x.AccountName3).Column("acct_name3").Nullable().Length(40);
        Map(x => x.NewAccountNumber).Column("new_acct_number").Nullable().Length(16);
        Map(x => x.CurrencyCode).Column("currency_code").Nullable().Length(4);
        Map(x => x.BranchCode).Column("branch_code").Nullable().Length(4);
    }
}

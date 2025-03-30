using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core.Models;

[Table("tbl_transaction")]
public class Transaction
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column("account_number")]
    public required string AccountNumber { get; set; }

    [Column("account_name")]
    public required string AccountName { get; set; }

    [Column("account_type")]
    public string? AccountType { get; set; }

    [Column("transaction_amount")]
    public decimal TransactionAmount { get; set; }

    [Column("charges")]
    public decimal Charges { get; set; }

    [Column("net_amount")]
    public decimal NetAmount { get; set; }

    [Column("is_match")]
    public bool IsMatch { get; set; }

    [Column("casa_acctname")]
    public required string CasaAccountName { get; set; }

    [Column("reason")]
    public string Reason { get; set; } = string.Empty;

    [Column("is_edited")]
    public bool IsEdited { get; set; }

    [Column("tran_num")]
    public string TransactionNumber { get; set; } = string.Empty;

    [Column("new_acct_num")]
    public required string NewAccountNumber { get; set; }

    [Column("user_id")]
    public required string UserId { get; set; }

    [Column("file_hash")]
    public required string FileHash { get; set; }

    [Column("file_name")]
    public required string FileName { get; set; }
}

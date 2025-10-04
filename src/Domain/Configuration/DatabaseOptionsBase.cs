using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Configuration;

public abstract class DatabaseOptionsBase
{
    [Required]
    [Url]
    public string Host { get; set; } = string.Empty;

    [Required]
    public int Port { get; set; }

    [Required]
    public string InitialCatalog { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [field: MaybeNull]
    public string ConnectionString
    {
        get
        {
            if (field is not null)
                return field;

            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = $"{Host},{Port}",
                InitialCatalog = InitialCatalog,
                UserID = UserId,
                Password = Password,
            };
            return field = builder.ConnectionString;
        }
    }
}

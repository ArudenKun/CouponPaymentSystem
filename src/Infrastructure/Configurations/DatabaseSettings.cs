using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using CouponPaymentSystem.Application.Common.Configurations;
using Microsoft.Data.SqlClient;

namespace CouponPaymentSystem.Infrastructure.Configurations;

internal sealed class DatabaseSettings : IDatabaseSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string InitialCatalog { get; init; }
    public required string UserId { get; init; }
    public required string Password { get; init; }
    public bool TrustServerCertificate { get; init; } = true;

    [JsonIgnore]
    [field: AllowNull, MaybeNull]
    public string ConnectionString =>
        field ??= new SqlConnectionStringBuilder
        {
            DataSource = $"{Host},{Port}",
            InitialCatalog = InitialCatalog,
            UserID = UserId,
            Password = Password,
            TrustServerCertificate = TrustServerCertificate,
        }.ToString();
}

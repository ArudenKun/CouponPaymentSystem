using System.Text.Json.Serialization;

namespace CouponPaymentSystem.Application.Common.Configurations;

public interface IDatabaseSettings
{
    string Host { get; init; }
    int Port { get; init; }
    string InitialCatalog { get; init; }
    string UserId { get; init; }
    string Password { get; init; }
    bool TrustServerCertificate { get; init; }

    [JsonIgnore]
    string ConnectionString { get; }
}

using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace CouponPaymentSystem.Core.Configuration.Options;

public sealed class DatabaseOptions
{
    private static readonly Decryptor Decryptor = new();

    public required string Host { get; init; }
    public int Port { get; init; } = 1125;
    public required string InitialCatalog { get; init; }
    public required string UserId { get; init; }
    public required string Password { get; init; }

    [JsonIgnore]
    [field: AllowNull, MaybeNull]
    public string ConnectionString =>
        field ??=
            $"Data Source={Host},{Port};Initial Catalog={InitialCatalog};User Id={UserId};Password={Decryptor.DecryptString(Password)};";
}

public class Decryptor
{
    public string DecryptString(string cipherText)
    {
        return cipherText;
    }
}

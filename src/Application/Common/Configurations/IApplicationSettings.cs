namespace CouponPaymentSystem.Application.Common.Configurations;

public interface IApplicationSettings
{
    string SysId { get; init; }
    int RetentionDays { get; init; }
    string Maker { get; init; }
    string Checker { get; init; }
    int MaxFileSize { get; init; }
    string FileNamePattern { get; init; }
    IReadOnlyList<string> FileExtensions { get; init; }
    string PingUrl { get; init; }
    long PingInterval { get; init; }
    IDatabaseSettings Aso { get; init; }
    IDatabaseSettings Cps { get; init; }
    IGefuSettings Gefu { get; init; }
}

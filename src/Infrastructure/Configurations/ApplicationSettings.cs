using Abp;
using Cogwheel;
using CouponPaymentSystem.Application.Common;
using CouponPaymentSystem.Application.Common.Configurations;
using CouponPaymentSystem.Application.Common.Extensions;
using Humanizer;

namespace CouponPaymentSystem.Infrastructure.Configurations;

internal sealed class ApplicationSettings : SettingsBase, IApplicationSettings, IShouldInitialize
{
    public ApplicationSettings(IPathManager pathManager)
        : base(pathManager.ConfigPath) { }

    public string SysId
    {
        get;
        init => field = value.PadLeftZero(3);
    } = "113";

    public int RetentionDays { get; init; } = 120;
    public string Maker { get; init; } = "CPS_MAKER";
    public string Checker { get; init; } = "CPS_CHECKER";
    public int MaxFileSize { get; init; } = 10;
    public string FileNamePattern { get; init; } = "^\\d{4}.*";
    public IReadOnlyList<string> FileExtensions { get; init; } = ["xls", "xlsx"];
    public string PingUrl { get; init; } = string.Empty;
    public long PingInterval { get; init; } = (long)5.Minutes().TotalSeconds;

    public DatabaseSettings Aso { get; init; } =
        new()
        {
            Host = "192.168.100.202",
            Port = 1433,
            InitialCatalog = "Cps",
            UserId = "sa",
            Password = "sa",
        };

    IDatabaseSettings IApplicationSettings.Aso
    {
        get => Aso;
        init { }
    }

    public DatabaseSettings Cps { get; init; } =
        new()
        {
            Host = "192.168.100.202",
            Port = 1433,
            InitialCatalog = "Cps",
            UserId = "sa",
            Password = "sa",
        };

    IDatabaseSettings IApplicationSettings.Cps
    {
        get => Cps;
        init { }
    }

    public GefuSettings Gefu { get; init; } = new();

    IGefuSettings IApplicationSettings.Gefu
    {
        get => Gefu;
        init { }
    }

    public void Initialize()
    {
        Load();
    }
}

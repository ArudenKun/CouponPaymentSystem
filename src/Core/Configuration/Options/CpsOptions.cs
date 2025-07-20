using Abp;
using Abp.Dependency;
using Castle.Core.Logging;
using Cogwheel;

namespace CouponPaymentSystem.Core.Configuration.Options;

public sealed class CpsOptions : SettingsBase, IShouldInitialize, ISingletonDependency
{
    private readonly ILogger _logger;

    public SystemId SysId { get; init; } = SystemId.From("113");

    public string Checker { get; init; } = "CPS_CHECKER";

    public DatabaseOptions Aso { get; init; } =
        new()
        {
            Host = "192.168.100.202",
            Port = 1433,
            InitialCatalog = "Cps",
            UserId = "sa",
            Password = "sa",
        };

    public DatabaseOptions Cps { get; init; } =
        new()
        {
            Host = "192.168.100.202",
            Port = 1433,
            InitialCatalog = "Cps",
            UserId = "sa",
            Password = "sa",
        };

    public CpsOptions(string path, ILogger logger)
        : base(path)
    {
        _logger = logger;
    }

    public void Initialize()
    {
        var isLoaded = Load();
        if (isLoaded)
        {
            _logger.Info("Loaded");
        }
        else
        {
            _logger.Warn("Could not load, Using defaults");
        }
    }

    public override void Save()
    {
        _logger.Info("Saving");
        base.Save();
        _logger.Info("Saved");
    }
}

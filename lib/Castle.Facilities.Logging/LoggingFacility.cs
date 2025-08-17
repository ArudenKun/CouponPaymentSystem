using System.Diagnostics;
using System.Reflection;
using Castle.Core.Internal;
using Castle.Core.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Conversion;

namespace Castle.Facilities.Logging;

/// <summary>
///   A facility for logging support.
/// </summary>
public class LoggingFacility : AbstractFacility
{
    private readonly string _customLoggerFactoryTypeName;
    private string _configFileName;

    private ITypeConverter _converter;

    private Type _loggerFactoryType;
    private LoggerLevel? _loggerLevel;
    private ILoggerFactory _loggerFactory;
    private string _logName;
    private bool _configuredExternally;

    /// <summary>
    ///   Initializes a new instance of the <see cref="LoggingFacility" /> class.
    /// </summary>
    public LoggingFacility() { }

    /// <summary>
    ///   Initializes a new instance of the <see cref="LoggingFacility" /> class using a custom LoggerImplementation
    /// </summary>
    /// <param name="customLoggerFactory"> The type name of the type of the custom logger factory. </param>
    /// <param name="configFile"> The configuration file that should be used by the chosen LoggerImplementation </param>
    public LoggingFacility(string customLoggerFactory, string configFile)
    {
        _customLoggerFactoryTypeName = customLoggerFactory;
        _configFileName = configFile;
    }

    public LoggingFacility LogUsing<TLoggerFactory>()
        where TLoggerFactory : ILoggerFactory
    {
        _loggerFactoryType = typeof(TLoggerFactory);
        return this;
    }

    public LoggingFacility LogUsing<TLoggerFactory>(TLoggerFactory loggerFactory)
        where TLoggerFactory : ILoggerFactory
    {
        _loggerFactoryType = typeof(TLoggerFactory);
        _loggerFactory = loggerFactory;
        return this;
    }

    public LoggingFacility ConfiguredExternally()
    {
        _configuredExternally = true;
        return this;
    }

    public LoggingFacility WithConfig(string configFile)
    {
        _configFileName = configFile ?? throw new ArgumentNullException(nameof(configFile));
        return this;
    }

    public LoggingFacility WithLevel(LoggerLevel level)
    {
        _loggerLevel = level;
        return this;
    }

    public LoggingFacility ToLog(string name)
    {
        _logName = name;
        return this;
    }

#if FEATURE_SYSTEM_CONFIGURATION
    /// <summary>
    ///   loads configuration from current AppDomain's config file (aka web.config/app.config)
    /// </summary>
    /// <returns> </returns>
    public LoggingFacility WithAppConfig()
    {
        configFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        return this;
    }
#endif

    protected override void Init()
    {
        SetUpTypeConverter();
        if (_loggerFactory == null)
        {
            ReadConfigurationAndCreateLoggerFactory();
        }
        RegisterLoggerFactory(_loggerFactory);
        RegisterDefaultILogger(_loggerFactory);
        RegisterSubResolver(_loggerFactory);
    }

    private void ReadConfigurationAndCreateLoggerFactory()
    {
        if (_loggerFactoryType == null)
        {
            _loggerFactoryType = ReadCustomLoggerType();
        }
        EnsureIsValidLoggerFactoryType();
        CreateProperLoggerFactory();
    }

    private Type ReadCustomLoggerType()
    {
        if (FacilityConfig != null)
        {
            var customLoggerType = FacilityConfig.Attributes["customLoggerFactory"];
            if (string.IsNullOrEmpty(customLoggerType) == false)
            {
                return _converter.PerformConversion<Type>(customLoggerType);
            }
        }
        if (_customLoggerFactoryTypeName != null)
        {
            return _converter.PerformConversion<Type>(_customLoggerFactoryTypeName);
        }
        return typeof(NullLogFactory);
    }

    private void EnsureIsValidLoggerFactoryType()
    {
        if (!_loggerFactoryType.Is<ILoggerFactory>())
        {
            throw new FacilityException(
                $"The specified type '{_loggerFactoryType}' does not implement ILoggerFactory."
            );
        }
    }

    private void CreateProperLoggerFactory()
    {
        Debug.Assert(_loggerFactoryType != null, "loggerFactoryType != null");

        var ctorArgs = GetLoggingFactoryArguments();
        _loggerFactory = _loggerFactoryType.CreateInstance<ILoggerFactory>(ctorArgs);
    }

    private string GetConfigFile()
    {
        if (_configFileName != null)
        {
            return _configFileName;
        }

        if (FacilityConfig != null)
        {
            return FacilityConfig.Attributes["configFile"];
        }
        return null;
    }

    private object[] GetLoggingFactoryArguments()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

        ConstructorInfo ctor;
        if (IsConfiguredExternally())
        {
            ctor = _loggerFactoryType.GetConstructor(flags, null, new[] { typeof(bool) }, null);
            if (ctor != null)
            {
                return new object[] { true };
            }
        }
        var configFile = GetConfigFile();
        if (configFile != null)
        {
            ctor = _loggerFactoryType.GetConstructor(flags, null, new[] { typeof(string) }, null);
            if (ctor != null)
            {
                return new object[] { configFile };
            }
        }

        var level = GetLoggingLevel();
        if (level != null)
        {
            ctor = _loggerFactoryType.GetConstructor(
                flags,
                null,
                new[] { typeof(LoggerLevel) },
                null
            );
            if (ctor != null)
            {
                return new object[] { level.Value };
            }
        }
        ctor = _loggerFactoryType.GetConstructor(flags, null, Type.EmptyTypes, null);
        if (ctor != null)
        {
            return new object[0];
        }
        throw new FacilityException(
            $"No support constructor found for logging type '{_loggerFactoryType}'"
        );
    }

    private bool IsConfiguredExternally()
    {
        if (_configuredExternally)
        {
            return true;
        }
        if (FacilityConfig != null)
        {
            var value = FacilityConfig.Attributes["configuredExternally"];
            if (value != null)
            {
                return _converter.PerformConversion<bool>(value);
            }
        }
        return false;
    }

    private LoggerLevel? GetLoggingLevel()
    {
        if (_loggerLevel.HasValue)
        {
            return _loggerLevel;
        }
        if (FacilityConfig != null)
        {
            var level = FacilityConfig.Attributes["loggerLevel"];
            if (level != null)
            {
                return _converter.PerformConversion<LoggerLevel>(level);
            }
        }
        return null;
    }

    private void RegisterDefaultILogger(ILoggerFactory factory)
    {
        if (factory is IExtendedLoggerFactory)
        {
            var defaultLogger = ((IExtendedLoggerFactory)factory).Create(_logName ?? "Default");
            Kernel.Register(
                Component
                    .For<IExtendedLogger>()
                    .NamedAutomatically("ilogger.default")
                    .Instance(defaultLogger),
                Component
                    .For<ILogger>()
                    .NamedAutomatically("ilogger.default.base")
                    .Instance(defaultLogger)
            );
        }
        else
        {
            Kernel.Register(
                Component
                    .For<ILogger>()
                    .NamedAutomatically("ilogger.default")
                    .Instance(factory.Create(_logName ?? "Default"))
            );
        }
    }

    private void RegisterLoggerFactory(ILoggerFactory factory)
    {
        if (factory is IExtendedLoggerFactory)
        {
            Kernel.Register(
                Component
                    .For<IExtendedLoggerFactory>()
                    .NamedAutomatically("iloggerfactory")
                    .Instance((IExtendedLoggerFactory)factory),
                Component
                    .For<ILoggerFactory>()
                    .NamedAutomatically("iloggerfactory.base")
                    .Instance(factory)
            );
        }
        else
        {
            Kernel.Register(
                Component
                    .For<ILoggerFactory>()
                    .NamedAutomatically("iloggerfactory")
                    .Instance(factory)
            );
        }
    }

    private void RegisterSubResolver(ILoggerFactory loggerFactory)
    {
        var extendedLoggerFactory = loggerFactory as IExtendedLoggerFactory;
        if (extendedLoggerFactory == null)
        {
            Kernel.Resolver.AddSubResolver(new LoggerResolver(loggerFactory, _logName));
            return;
        }
        Kernel.Resolver.AddSubResolver(new LoggerResolver(extendedLoggerFactory, _logName));
    }

    private void SetUpTypeConverter()
    {
        _converter =
            Kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey) as IConversionManager;
    }
}

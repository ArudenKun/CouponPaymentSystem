using System.Diagnostics;
using Castle.Core;
using Castle.Core.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace Castle.Facilities.Logging;

/// <summary>
///   Custom resolver used by Windsor. It gives
///   us some contextual information that we use to set up a logging
///   before satisfying the dependency
/// </summary>
public class LoggerResolver : ISubDependencyResolver
{
    private readonly IExtendedLoggerFactory _extendedLoggerFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly string _logName;

    public LoggerResolver(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    public LoggerResolver(IExtendedLoggerFactory extendedLoggerFactory)
    {
        if (extendedLoggerFactory == null)
        {
            throw new ArgumentNullException(nameof(extendedLoggerFactory));
        }

        _extendedLoggerFactory = extendedLoggerFactory;
    }

    public LoggerResolver(ILoggerFactory loggerFactory, string name)
        : this(loggerFactory)
    {
        _logName = name;
    }

    public LoggerResolver(IExtendedLoggerFactory extendedLoggerFactory, string name)
        : this(extendedLoggerFactory)
    {
        _logName = name;
    }

    public bool CanResolve(
        CreationContext context,
        ISubDependencyResolver parentResolver,
        ComponentModel model,
        DependencyModel dependency
    )
    {
        return dependency.TargetType == typeof(ILogger)
            || dependency.TargetType == typeof(IExtendedLogger);
    }

    public object Resolve(
        CreationContext context,
        ISubDependencyResolver parentResolver,
        ComponentModel model,
        DependencyModel dependency
    )
    {
        Debug.Assert(CanResolve(context, parentResolver, model, dependency));
        if (_extendedLoggerFactory != null)
        {
            return string.IsNullOrEmpty(_logName)
                ? _extendedLoggerFactory.Create(model.Implementation)
                : _extendedLoggerFactory
                    .Create(_logName)
                    .CreateChildLogger(model.Implementation.FullName);
        }

        Debug.Assert(_loggerFactory != null);
        return string.IsNullOrEmpty(_logName)
            ? _loggerFactory.Create(model.Implementation)
            : _loggerFactory.Create(_logName).CreateChildLogger(model.Implementation.FullName);
    }
}

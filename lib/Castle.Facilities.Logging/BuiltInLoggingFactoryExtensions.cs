using Castle.Core.Logging;

namespace Castle.Facilities.Logging;

public static class BuiltInLoggingFactoryExtensions
{
    public static LoggingFacility LogUsingNullLogger(this LoggingFacility loggingFacility) =>
        loggingFacility.LogUsing<NullLogFactory>();

    public static LoggingFacility LogUsingConsoleLogger(this LoggingFacility loggingFacility) =>
        loggingFacility.LogUsing<ConsoleFactory>();

#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public static LoggingFacility LogUsingDiagnosticsLogger(this LoggingFacility loggingFacility) =>
        loggingFacility.LogUsing<DiagnosticsLoggerFactory>();

    public static LoggingFacility LogUsingTraceLogger(this LoggingFacility loggingFacility) =>
        loggingFacility.LogUsing<TraceLoggerFactory>();
}

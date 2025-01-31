using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Infrastructure.Logging.Sinks;

public record ConsoleLoggingOptions(
    LogEventLevel RestrictedToMinimumLevel = ConsoleLoggingOptionsConstants.DefaultRestrictedToMinimumLevel,
    string OutputTemplate = ConsoleLoggingOptionsConstants.DefaultOutputTemplate,
    IFormatProvider? FormatProvider = null,
    LoggingLevelSwitch? LevelSwitch = null,
    LogEventLevel? StandardErrorFromLevel = null,
    ConsoleTheme? Theme = null,
    bool ApplyThemeToRedirectedOutput = false,
    object? SyncRoot = null)
{

    public static ConsoleLoggingOptions Default() => new();
}

public static class ConsoleLoggingOptionsConstants
{
    public const LogEventLevel DefaultRestrictedToMinimumLevel = LogEventLevel.Debug;
    public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
}
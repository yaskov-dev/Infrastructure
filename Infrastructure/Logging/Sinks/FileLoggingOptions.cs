using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;

namespace Infrastructure.Logging.Sinks;

/// <summary>
/// 
/// </summary>
/// <param name="Path"></param>
/// <param name="WithFileReader">Adding FileLogsReader in DI</param>
/// <param name="RestrictedToMinimumLevel"></param>
/// <param name="OutputTemplate"></param>
/// <param name="FormatProvider"></param>
/// <param name="FileSizeLimitBytes"></param>
/// <param name="LevelSwitch"></param>
/// <param name="Buffered"></param>
/// <param name="Shared"></param>
/// <param name="FlushToDiskInterval"></param>
/// <param name="RollingInterval"></param>
/// <param name="RollOnFileSizeLimit"></param>
/// <param name="RetainedFileCountLimit"></param>
/// <param name="Encoding"></param>
/// <param name="Hooks"></param>
/// <param name="RetainedFileTimeLimit"></param>
public record FileLoggingOptions(
    string Path = "Logs/logs-.txt",
    bool WithFileReader = true,
    LogEventLevel RestrictedToMinimumLevel = LogEventLevel.Debug,
    string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
    IFormatProvider? FormatProvider = null,
    long? FileSizeLimitBytes = 1L * 1024 * 1024 * 1024 // 1GB
                               * 5, // 5GB
    LoggingLevelSwitch? LevelSwitch = null,
    bool Buffered = false,
    bool Shared = false,
    TimeSpan? FlushToDiskInterval = null,
    RollingInterval RollingInterval = RollingInterval.Day,
    bool RollOnFileSizeLimit = false,
    int? RetainedFileCountLimit = 31, // month
    Encoding? Encoding = null,
    FileLifecycleHooks? Hooks = null,
    TimeSpan? RetainedFileTimeLimit = null)
{
    public static FileLoggingOptions Default() => new();
}
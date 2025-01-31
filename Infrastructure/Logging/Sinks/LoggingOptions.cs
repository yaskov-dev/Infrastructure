#nullable enable

namespace Infrastructure.Logging.Sinks;

public record LoggingOptions(
    ConsoleLoggingOptions? ConsoleOptions = null,
    FileLoggingOptions? FileOptions = null,
    ElasticSearchLoggingOptions? ElasticSearchOptions = null)
{
    public static LoggingOptions DefaultLocal => new(
        ConsoleLoggingOptions.Default(),
        FileLoggingOptions.Default());
    
    public static LoggingOptions DefaultCloud => new(
        ConsoleLoggingOptions.Default(),
        FileLoggingOptions.Default(),
        ElasticSearchLoggingOptions.Default());
}
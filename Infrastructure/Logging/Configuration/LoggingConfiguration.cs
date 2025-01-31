using Elastic.Serilog.Sinks;
using Infrastructure.Logging.Sinks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Infrastructure.Logging.Configuration;

public static class LoggingConfiguration
{
    /// <summary>
    /// Configures logger
    /// </summary>
    /// <param name="services">DI where logger will be added</param>
    /// <param name="serviceName">Service name to configure logger for</param>
    /// <param name="loggingOptions">Options to configure logging</param>
    public static void ConfigureSerilog(
        this IServiceCollection services, 
        string serviceName,
        LoggingOptions loggingOptions)
    {
        services.AddSingleton(loggingOptions);
        
        var configuration = new LoggerConfiguration();
        if (loggingOptions.ConsoleOptions != null)
            configuration.ConfigureConsole(loggingOptions.ConsoleOptions);
        if (loggingOptions.FileOptions != null)
        {
            configuration.ConfigureFile(loggingOptions.FileOptions);
            if (loggingOptions.FileOptions.WithFileReader)
                services.AddSingleton<IFileLogsReader, FileLogsReader>();
        }
        if (loggingOptions.ElasticSearchOptions != null)
            configuration.ConfigureElastic(loggingOptions.ElasticSearchOptions); // TODO: make elastic config better

        Log.Logger = configuration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Service", serviceName)
            .CreateLogger();
        services.AddSerilog();
    }

    private static void ConfigureConsole(this LoggerConfiguration configuration, ConsoleLoggingOptions opt)
    {
        configuration.WriteTo.Console(
            restrictedToMinimumLevel: opt.RestrictedToMinimumLevel,
            outputTemplate: opt.OutputTemplate,
            formatProvider: opt.FormatProvider,
            levelSwitch: opt.LevelSwitch,
            standardErrorFromLevel: opt.StandardErrorFromLevel,
            theme: opt.Theme,
            applyThemeToRedirectedOutput: opt.ApplyThemeToRedirectedOutput,
            syncRoot: opt.SyncRoot
        );
    }

    private static void ConfigureFile(this LoggerConfiguration configuration, FileLoggingOptions opt)
    {
        configuration.WriteTo.File(
            path: opt.Path,
            restrictedToMinimumLevel: opt.RestrictedToMinimumLevel,
            outputTemplate: opt.OutputTemplate,
            formatProvider: opt.FormatProvider,
            fileSizeLimitBytes: opt.FileSizeLimitBytes,
            levelSwitch: opt.LevelSwitch,
            buffered: opt.Buffered,
            shared: opt.Shared,
            flushToDiskInterval: opt.FlushToDiskInterval,
            rollingInterval: opt.RollingInterval,
            rollOnFileSizeLimit: opt.RollOnFileSizeLimit,
            retainedFileCountLimit: opt.RetainedFileCountLimit,
            encoding: opt.Encoding,
            hooks: opt.Hooks,
            retainedFileTimeLimit: opt.RetainedFileTimeLimit
        );
    }

    private static void ConfigureElastic(this LoggerConfiguration configuration, ElasticSearchLoggingOptions opt)
    {
        configuration.WriteTo.Elasticsearch(
            nodes: opt.Nodes,
            configureOptions: opt.ConfigureOptions,
            configureTransport: opt.ConfigureTransport,
            useSniffing: opt.UseSniffing,
            levelSwitch: opt.LevelSwitch,
            restrictedToMinimumLevel: opt.RestrictedToMinimumLevel
        );
    }
}
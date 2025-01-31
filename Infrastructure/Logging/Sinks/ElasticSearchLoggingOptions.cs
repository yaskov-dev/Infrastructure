using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Serilog.Core;
using Serilog.Events;

namespace Infrastructure.Logging.Sinks;

public record ElasticSearchLoggingOptions(
    ICollection<Uri> Nodes,
    Action<ElasticsearchSinkOptions>? ConfigureOptions = null,
    Action<TransportConfigurationDescriptor>? ConfigureTransport = null,
    bool UseSniffing = false,
    LoggingLevelSwitch? LevelSwitch = null,
    LogEventLevel RestrictedToMinimumLevel = ElasticsearchLoggingOptionsConstants.DefaultRestrictedToMinimumLevel)
{

    public static ElasticSearchLoggingOptions Default() => new(
        Nodes: new[] { ElasticsearchLoggingOptionsConstants.DefaultNode },
        ConfigureOptions: ElasticsearchLoggingOptionsConstants.DefaultOptions,
        ConfigureTransport: ElasticsearchLoggingOptionsConstants.DefaultTransport);
}

public static class ElasticsearchLoggingOptionsConstants
{
    public static Uri DefaultNode = new("http://localhost:9200");
    public const LogEventLevel DefaultRestrictedToMinimumLevel = LevelAlias.Minimum;

    public static Action<ElasticsearchSinkOptions> DefaultOptions = opt => { }; // TODO
    public static Action<TransportConfigurationDescriptor> DefaultTransport = transport => { };
}
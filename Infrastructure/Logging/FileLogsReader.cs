using System.Text;
using Infrastructure.ActionResults;
using Infrastructure.Logging.Sinks;
using Serilog;

namespace Infrastructure.Logging;

public interface IFileLogsReader
{
    Task<Result<string>> Read(DateOnly date);
}

public class FileLogsReader : IFileLogsReader
{
    private readonly LoggingOptions loggingOptions;

    public FileLogsReader(LoggingOptions loggingOptions)
    {
        this.loggingOptions = loggingOptions;
    }
    
    public async Task<Result<string>> Read(DateOnly date)
    {
        var fileLoggingOptions = loggingOptions.FileOptions;
        if (fileLoggingOptions == null)
            return Results.BadRequest<string>("File logging is not configured.");

        var sb = new StringBuilder();
        var logFilePath = BuildFilePath(fileLoggingOptions.Path, fileLoggingOptions.RollingInterval, date);

        if (!File.Exists(logFilePath)) 
            return Results.NotFound<string>("No logs found");
        
        await using var stream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: 4096, useAsync: true);
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync() is { } line)
        {
            sb.AppendLine(line);
        }

        return Results.Ok(sb.ToString());
    }

    private string BuildFilePath(string filePath, RollingInterval rollingInterval, DateOnly date)
    {
        var appended = rollingInterval switch
        {
            RollingInterval.Infinite => "",
            RollingInterval.Year => date.ToString("yyyy"),
            RollingInterval.Month => date.ToString("yyyyMM"),
            RollingInterval.Day => date.ToString("yyyyMMdd"),
            _ => throw new ArgumentException($"Unsupported {nameof(RollingInterval)}: {rollingInterval.ToString()}")
        };
        if (appended.Length == 0)
            return filePath;
        
        var extension = Path.GetExtension(filePath) ?? "";
        return filePath[..^extension.Length]
               + appended 
               + filePath[^extension.Length..];
    }
}
namespace Infrastructure.Shared;

public static class BufferedFileReader
{
    public static async IAsyncEnumerable<string> ReadLinesAsync(string filePath, int bufferSize = 4096)
    {
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: bufferSize, useAsync: true);
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync() is { } line)
            yield return line;
    }
    
    public static async IAsyncEnumerable<string> ReadNotEmptyOrWhiteSpacedLinesAsync(string filePath, int bufferSize = 4096)
    {
        await foreach (var line in ReadLinesAsync(filePath, bufferSize))
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                continue;

            yield return line;
        }
    }
    
    public static IEnumerable<string> ReadLines(string filePath, int bufferSize = 4096)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: bufferSize, useAsync: true);
        using var reader = new StreamReader(stream);
        while (reader.ReadLine() is { } line)
            yield return line;
    }
    
    public static IEnumerable<string> ReadNotEmptyOrWhiteSpacedLines(string filePath, int bufferSize = 4096)
    {
        foreach (var line in ReadLines(filePath, bufferSize))
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                continue;

            yield return line;
        }
    }
}
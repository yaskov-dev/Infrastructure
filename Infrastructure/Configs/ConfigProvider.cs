using System.Reflection;
using Infrastructure.Configs.Sinks;
using Infrastructure.Shared;

namespace Infrastructure.Configs;

public interface IConfigProvider
{
    /// <summary>
    /// Reads config from sources. Sources is ordered by their priorities, Ascending
    /// </summary>
    /// <param name="configOptions">Sources</param>
    /// <typeparam name="T">Config type</typeparam>
    /// <returns>Filled config</returns>
    T Get<T>(params ConfigOption[] configOptions)
        where T : class, new();
}

public class ConfigProvider : IConfigProvider
{
    private readonly string separator;
    
    public ConfigProvider(string separator = "=")
    {
        this.separator = separator;
    }

    public T Get<T>(params ConfigOption[] configOptions) 
        where T : class, new()
    {
        if (configOptions.Length == 0)
            throw new ConfigException("ConfigOptions can not be empty.");
        
        var config = new T();
        var properties = typeof(T).GetProperties();
        foreach (var configOption in configOptions)
        {
            if (configOption is FileConfigOption fileConfigOption)
                FillConfig(fileConfigOption, properties, config);
            else if (configOption is EnvironmentConfigOption environmentConfigOption)
                FillConfig(environmentConfigOption, properties, config);
            else
                throw new NotSupportedException($"ConfigOption is not supported: {configOption.GetType().Name}");
        }

        return config;
    }
    
    private void FillConfig<T>(EnvironmentConfigOption environmentConfigOption, PropertyInfo[] properties, T config)
    {
        foreach (var property in properties)
        {
            var envVar = Environment.GetEnvironmentVariable(property.Name);
            if (envVar == null)
            {
                if (environmentConfigOption.ThrowIfVariableNotExists)
                    throw new ConfigException($"Environment does not contains variable: {property.Name} and option is true: {nameof(environmentConfigOption.ThrowIfVariableNotExists)}");
                
                continue;
            }

            property.SetTypedValue(config!, envVar);
        }
    }

    private void FillConfig<T>(FileConfigOption fileConfigOption, PropertyInfo[] properties, T config)
    {
        var filePath = fileConfigOption.IsPathRelativeToCsproj
            ? BuildPathFromCsproj(fileConfigOption.Path)
            : fileConfigOption.Path;
        if (!File.Exists(filePath))
            throw new ConfigException($"Can not find ConfigFile. Path: {filePath}");
        
        FillConfigFromFile(filePath, properties, config);
    }
    
    private void FillConfigFromFile<T>(string filePath, PropertyInfo[] properties, T config)
    {
        foreach (var line in BufferedFileReader.ReadNotEmptyOrWhiteSpacedLines(filePath))
        {
            var separatorIndex = line.IndexOf(separator, StringComparison.InvariantCulture);
            if (separatorIndex == -1)
                throw new ConfigException($"ConfigLine does not contains separator({separator}). Line: {line}");

            var propName = line.AsSpan()[..(separatorIndex - 1)].Trim().ToString();
            var propValue = line.AsSpan()[(separatorIndex + 1)..].Trim().ToString();
            if (propName.Length == 0 || propValue.Length == 0)
                throw new ConfigException($"ConfigLine does not contains prop or value. Line: {line}");

            var property = properties.FirstOrDefault(e => e.Name == propName);
            if (property == null)
                continue;
            
            property.SetTypedValue(config!, propValue);
        }
    }

    private string BuildPathFromCsproj(string path)
    {
        return path.StartsWith(Path.PathSeparator)
            ? $"{Environment.CurrentDirectory}{path}"
            : $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}{path}";
    }
}
using System.Reflection;
using Infrastructure.Application;
using Infrastructure.Configs.Sources;
using Infrastructure.Shared;

namespace Infrastructure.Configs;

public static class ConfigProvider
{
    public static T Get<T>() 
        where T : class, new()
        => Get<T>(ConfigSources.Default);
    
    public static  T Get<T>(ConfigSource[] configSources) 
        where T : class, new()
    {
        if (configSources.Length == 0)
            throw new ConfigException("ConfigOptions can not be empty.");
        
        var config = new T();
        var properties = typeof(T).GetProperties();
        foreach (var configOption in configSources)
        {
            if (configOption is FileConfigSource fileConfigSource)
                FillConfig(fileConfigSource, properties, config);
            else if (configOption is EnvironmentConfigSource environmentConfigSource)
                FillConfig(environmentConfigSource, properties, config);
            else
                throw new NotSupportedException($"ConfigOption is not supported: {configOption.GetType().Name}");
        }

        return config;
    }
    
    private static void FillConfig<T>(EnvironmentConfigSource environmentConfigSource, PropertyInfo[] properties, T config)
    {
        foreach (var property in properties)
        {
            var envNameAttr = property.GetCustomAttribute<EnvironmentVariableAttribute>();
            if (envNameAttr?.Name == null)
                throw new ConfigException($"Config variable does not contains Environment label. Need to Add attribute: {nameof(EnvironmentVariableAttribute)} to variable: {property.Name}");
                
            var envVar = Environment.GetEnvironmentVariable(envNameAttr.Name);
            if (envVar == null)
            {
                if (environmentConfigSource.ThrowIfVariableNotExists)
                    throw new ConfigException($"Environment does not contains variable: {envNameAttr.Name} and option is true: {nameof(environmentConfigSource.ThrowIfVariableNotExists)}");
                
                continue;
            }

            property.SetTypedValue(config!, envVar);
        }
    }

    private static void FillConfig<T>(FileConfigSource fileConfigSource, PropertyInfo[] properties, T config)
    {
        var filePath = fileConfigSource.IsPathRelativeToCsproj
            ? BuildPathFromCsproj(fileConfigSource.Path)
            : fileConfigSource.Path;
        if (!File.Exists(filePath))
            throw new ConfigException($"Can not find ConfigFile. Path: {filePath}");
        
        FillConfigFromFile(filePath, fileConfigSource.ValuesSeparator, properties, config);
    }
    
    private static void FillConfigFromFile<T>(string filePath, string valuesSeparator, PropertyInfo[] properties, T config)
    {
        foreach (var line in BufferedFileReader.ReadNotEmptyOrWhiteSpacedLines(filePath))
        {
            var separatorIndex = line.IndexOf(valuesSeparator, StringComparison.InvariantCulture);
            if (separatorIndex == -1)
                throw new ConfigException($"ConfigLine does not contains separator({valuesSeparator}). Line: {line}");

            var propName = line.AsSpan()[..(separatorIndex - 1)].Trim().ToString();
            var propValue = line.AsSpan()[(separatorIndex + 1)..].Trim().ToString();
            if (propName.Length == 0 || propValue.Length == 0)
                throw new ConfigException($"ConfigLine does not contains prop or value. Line: {line}");

            var property = properties.FirstOrDefault(e => e.Name.ToLower() == propName.ToLower());
            if (property == null)
                continue;
            
            property.SetTypedValue(config!, propValue);
        }
    }

    private static string BuildPathFromCsproj(string path)
    {
        return path.StartsWith(Path.PathSeparator)
            ? $"{Environment.CurrentDirectory}{path}"
            : $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}{path}";
    }
}
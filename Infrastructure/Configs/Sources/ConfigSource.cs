namespace Infrastructure.Configs.Sources;

public abstract record class ConfigSource
{
}

public static class ConfigSources
{
    public static ConfigSource[] Development => new ConfigSource[] { FileConfigSource.Cloud, FileConfigSource.Local, EnvironmentConfigSource.Default };
    public static ConfigSource[] Production => new ConfigSource[] { FileConfigSource.Cloud, EnvironmentConfigSource.Default };
}
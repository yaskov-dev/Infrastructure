namespace Infrastructure.Configs.Sources;

public abstract record class ConfigSource
{
}

public static class ConfigSources
{
    public static ConfigSource[] Default => new ConfigSource[] { FileConfigSource.Cloud, FileConfigSource.Local, EnvironmentConfigSource.Default };
}
namespace Infrastructure.Configs.Sinks;

public abstract record class ConfigOption
{
}

public static class ConfigOptions
{
    public static ConfigOption[] Development => new ConfigOption[] { FileConfigOption.Cloud, FileConfigOption.Local, EnvironmentConfigOption.Default };
    public static ConfigOption[] Production => new ConfigOption[] { FileConfigOption.Cloud, EnvironmentConfigOption.Default };
}
namespace Infrastructure.Configs;

public class ConfigException : ApplicationException
{
    public ConfigException(string error) : base($"Config is incorrect. {error}")
    {}
}
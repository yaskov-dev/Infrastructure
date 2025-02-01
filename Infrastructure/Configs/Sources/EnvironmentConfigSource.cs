namespace Infrastructure.Configs.Sources;

public record class EnvironmentConfigSource(
    bool ThrowIfVariableNotExists = false
) : ConfigSource
{
    public static EnvironmentConfigSource Default => new();
}
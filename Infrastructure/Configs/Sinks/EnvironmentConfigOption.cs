#nullable enable

namespace Infrastructure.Configs.Sinks;

public record class EnvironmentConfigOption(
    bool ThrowIfVariableNotExists = false
) : ConfigOption
{
    public static EnvironmentConfigOption Default => new();
}
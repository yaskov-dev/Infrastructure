#nullable enable

namespace Infrastructure.Configs.Sinks;

public record class FileConfigOption(
    string Path = "Configs/config",
    bool IsPathRelativeToCsproj = true
) : ConfigOption
{
    public static FileConfigOption Cloud => new("Configs/cloudConfig", true);
    public static FileConfigOption Local => new("Configs/localConfig", true);
}
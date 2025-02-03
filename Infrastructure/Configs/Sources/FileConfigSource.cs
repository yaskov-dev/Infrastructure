namespace Infrastructure.Configs.Sources;

public record class FileConfigSource(
    string Path = "Configs/config",
    bool IsPathRelativeToCsproj = true,
    bool IsNecessary = true,
    string ValuesSeparator = "="
) : ConfigSource
{
    public static FileConfigSource Cloud => new("Configs/cloudConfig", true, true);
    public static FileConfigSource Local => new("Configs/localConfig", true, false);
}
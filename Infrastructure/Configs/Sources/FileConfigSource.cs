namespace Infrastructure.Configs.Sources;

public record class FileConfigSource(
    string Path = "Configs/config",
    bool IsPathRelativeToCsproj = true
) : ConfigSource
{
    public static FileConfigSource Cloud => new("Configs/cloudConfig", true);
    public static FileConfigSource Local => new("Configs/localConfig", true);
}
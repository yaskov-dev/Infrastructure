using Infrastructure.Configs.Sources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.Configs.Configuration;

public static class ConfigsConfiguration
{
    public static void AddColdConfig<TConfig>(
        this IServiceCollection services)
        where TConfig : class, new()
    {
        services.AddSingleton<TConfig>(_ => ConfigProvider.Get<TConfig>(ConfigSources.Default));
    }
    
    public static void AddColdConfig<TConfig>(
        this IServiceCollection services,
        ConfigSource[] configOptions)
    where TConfig : class, new()
    {
        services.AddSingleton<TConfig>(_ => ConfigProvider.Get<TConfig>(configOptions));
    }
}
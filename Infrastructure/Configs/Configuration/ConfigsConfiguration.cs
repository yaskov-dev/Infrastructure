using Infrastructure.Configs.Sources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.Configs.Configuration;

public static class ConfigsConfiguration
{
    public static void AddColdConfig<TConfig>(
        this IServiceCollection services,
        params ConfigSource[] configOptions)
    where TConfig : class, new()
    {
        services.TryAddSingleton<IConfigProvider, ConfigProvider>();
        services.AddSingleton<TConfig>(scope =>
        {
            var configProvider = scope.GetRequiredService<IConfigProvider>();
            return configProvider.Get<TConfig>(configOptions);
        });
    }
}
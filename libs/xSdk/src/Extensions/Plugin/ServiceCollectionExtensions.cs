using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Hosting;

namespace xSdk.Extensions.Plugin;

public static class ServiceCollectionExtensions
{
    private static bool IsLocked = false;

    public static IServiceCollection AddPluginServices(this IServiceCollection services)
    {
        services.Replace(
            ServiceDescriptor.Singleton(provider =>
            {
                IsLocked = true;
                return SlimHostInternal.Instance.PluginSystem;
            })
        );

        return services;
    }

    internal static IServiceCollection AddSlimPluginServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IPluginService>(provider =>
        {
            if (!IsLocked)
            {
                var service = ActivatorUtilities.CreateInstance<PluginService>(provider);
                return service;
            }
            else
            {
                throw new SdkException("SlimPluginService is locked and cannot be used");
            }
        });

        return services;
    }
}

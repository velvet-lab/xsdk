using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Options;

namespace xSdk.Hosting.Managers;

internal static class LoggingManager
{
    internal static IServiceCollection AddSdkLogging(this IServiceCollection services, SlimHost slimHost, bool invokePlugins)
    {
        

        if (invokePlugins)
        {
            services
            .AddLogging(builder =>
            {
                slimHost.ConfigurePluginHost(plugin => plugin.ConfigureLogging(builder));
            });
        }
        else
        {
            services.AddLogging();
        }

        services
            .AddSingleton(typeof(ILogger<>), provider =>
            {
                var factory = provider.GetRequiredService<ILoggerFactory>();
                var options = provider.GetService<IOptions<EnvironmentOptions>>()?.Value ?? throw new InvalidOperationException("Environment Options could not loaded");

                LogManager.Initialize(factory);
                return LogManager.CreateLogger(typeof(ILogger<>));

            });

        return services;
    }

    internal static IServiceCollection AddLoggerFactory(this IServiceCollection services, SlimHost slimHost)
    {
        services.AddSingleton<ILoggerFactory>(provider =>
        {
            if (!LogManager.IsInitialized)
            {
                var options = provider.GetService<IOptions<EnvironmentOptions>>()?.Value ?? throw new InvalidOperationException("Environment Options could not loaded");
                LogManager.Initialize(options.LogLevel);
            }

            return LogManager.Factory;
        });

        return services;
    }
}

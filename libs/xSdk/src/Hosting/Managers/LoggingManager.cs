using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Options;

namespace xSdk.Hosting.Managers;

internal static class LoggingManager
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddHostLogging(SlimHost slimHost, EnvironmentOptions options)
        {
            services
                .AddLogging(loggingBuilder =>
                 {
                     loggingBuilder
                         .ClearProviders()
                         .SetMinimumLevel(options.LogLevel);

                     var logBuilder = new LogBuilder(loggingBuilder, options.LogLevel);

                     // Apply plugin configurations
                     slimHost
                         .ConfigurePluginHost(plugin => plugin.ConfigureLogging(logBuilder));

                     logBuilder.Build();
                 });

            return services;
        }
    }
}

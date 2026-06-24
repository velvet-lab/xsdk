using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

internal static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLoggingQueue()
        {
            services
                .AddSingleton<ILoggerFactory, QueueLoggerFactory>()
                .AddSingleton(typeof(ILogger<>), typeof(QueueLogger<>));

            return services;
        }
    }
}

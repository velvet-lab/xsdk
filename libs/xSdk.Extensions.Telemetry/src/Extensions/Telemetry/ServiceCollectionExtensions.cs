using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Extensions.Telemetry
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelemetryServices(this IServiceCollection services) => services.AddTelemetryServices(null);

        public static IServiceCollection AddTelemetryServices(this IServiceCollection services, Action<TelemetrySetup> configure)
        {
            services.TryAddSingleton<ITelemetryService>(provider =>
            {
                var service = ActivatorUtilities.CreateInstance<TelemetryService>(provider);

                return service;
            });

            return services;
        }
    }
}

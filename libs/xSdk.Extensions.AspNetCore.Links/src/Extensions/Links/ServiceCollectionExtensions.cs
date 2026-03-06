using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Extensions.Links
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLinksService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<ILinksService>(provider =>
            {
                var options = new LinksOptions();
                SlimHost.Instance.PluginSystem.Invoke<ILinksPluginBuilder>(x => x.ConfigureLinks(options));

                var service = ActivatorUtilities.CreateInstance<LinksService>(provider, options);

                return service;
            });

            return services;
        }
    }
}

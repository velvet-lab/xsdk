using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Plugins.WebSecurity
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder EnableWebSecurity(this IHostBuilder builder)
        {
            builder
                .RegisterSetup<WebSecuritySetup>()
                .EnablePlugin<WebSecurityPlugin>();

            return builder;
        }

        public static IHostBuilder EnableWebSecurity<TPluginBuilder>(this IHostBuilder builder)
            where TPluginBuilder : IWebSecurityPluginBuilder
        {
            builder
                .EnableWebSecurity()
                .EnablePlugin<TPluginBuilder>();

            return builder;
        }
    }
}

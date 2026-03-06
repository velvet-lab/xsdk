using xSdk.Extensions.Links;
using xSdk.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Plugins.Links
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder EnableLinks(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .EnablePlugin<LinksPlugin>();
        }

        public static IHostBuilder EnableLinks<TPluginBuilder>(this IHostBuilder hostBuilder)
            where TPluginBuilder : ILinksPluginBuilder
        {
            return hostBuilder
                .EnableLinks()
                .EnablePlugin<TPluginBuilder>();
        }
    }
}

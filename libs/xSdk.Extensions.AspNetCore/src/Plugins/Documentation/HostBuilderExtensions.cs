using Microsoft.Extensions.Hosting;
using xSdk.Hosting;

namespace xSdk.Plugins.Documentation;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableDocumentation(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .RegisterSetup<DocumentationSetup>()
            .EnablePlugin<DocumentationPlugin>();
    }

    public static IHostBuilder EnableDocumentation<TPluginBuilder>(this IHostBuilder hostBuilder)
        where TPluginBuilder : IDocumentationPluginBuilder
    {
        return hostBuilder
            .EnableDocumentation()
            .EnablePlugin<TPluginBuilder>();
    }
}

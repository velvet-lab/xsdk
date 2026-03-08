using Microsoft.Extensions.Hosting;
using xSdk.Hosting;

namespace xSdk.Plugins.WebApi;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableWebApi(this IHostBuilder builder)
    {
        builder
            .EnablePlugin<WebApiPlugin>();

        return builder;
    }

    public static IHostBuilder EnableWebApi<TPluginBuilder>(this IHostBuilder builder)
        where TPluginBuilder : IWebApiPluginBuilder
    {
        builder
            .EnableWebApi()
            .EnablePlugin<TPluginBuilder>();

        return builder;
    }
}

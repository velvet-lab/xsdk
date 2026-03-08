using Microsoft.Extensions.Hosting;
using xSdk.Hosting;

namespace xSdk.Plugins.CloudEvents;

public static class HostBuilderExtensions
{
    public static IHostBuilder EnableCloudEvents(this IHostBuilder hostBuilder)
    {
        hostBuilder.EnablePlugin<CloudEventPlugin>();

        return hostBuilder;
    }
}

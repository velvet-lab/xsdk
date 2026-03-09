using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Links;
using xSdk.Hosting;

namespace xSdk.Plugins.Links;

internal class LinksPlugin : PluginBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLinksService();
    }
}

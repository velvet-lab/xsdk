using xSdk.Extensions.Links;
using xSdk.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Plugins.Links
{
    internal class LinksPlugin : PluginBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLinksService();
        }
    }
}

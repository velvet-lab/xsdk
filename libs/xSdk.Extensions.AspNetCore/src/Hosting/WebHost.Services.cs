using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static void ConfigureWebHostServicesWithContext(WebHostBuilderContext context, IServiceCollection services)
    {
        SlimHost.Instance.PluginSystem.Invoke<WebHostPluginBase>(x => x.ConfigureServices(context, services));
    }
}

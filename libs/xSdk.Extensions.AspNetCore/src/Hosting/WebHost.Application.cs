using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace xSdk.Hosting
{
    public static partial class WebHost
    {
        private static void ConfigureApplicationWithContext(WebHostBuilderContext context, IApplicationBuilder app)
        {
            Logger.Info("Configuring application services");

            var plugins = SlimHost.Instance.PluginSystem.GetPlugins<WebHostPluginBase>();

            // Only the first Plugin needs to configure defaults
            var firstPlugin = plugins.FirstOrDefault();
            if (firstPlugin != null)
            {
                firstPlugin.ConfigureDefaults(context, app);
            }

            foreach (var plugin in plugins)
            {
                plugin.Configure(context, app);
            }

            app.UseEndpoints(builder =>
            {
                foreach (var plugin in plugins)
                {
                    plugin.ConfigureEndpoint(builder);
                }
            });
        }
    }
}

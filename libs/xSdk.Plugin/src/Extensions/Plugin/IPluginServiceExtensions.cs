namespace xSdk.Extensions.Plugin
{
    public static class IPluginServiceExtensions
    {
        public static bool Invoke<TPlugin>(this IPluginService pluginService, Action<TPlugin> factory)
        {
            var plugins = pluginService.GetPlugins<TPlugin>();
            foreach (var plugin in plugins)
            {
                factory?.Invoke(plugin);
            }
            return plugins.Any();
        }
    }
}

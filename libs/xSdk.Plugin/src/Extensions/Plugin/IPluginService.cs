using System.Reflection;

namespace xSdk.Extensions.Plugin
{
    public interface IPluginService
    {
        TPlugin? GetPlugin<TPlugin>() => GetPluginAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

        Task<TPlugin?> GetPluginAsync<TPlugin>(CancellationToken token = default);

        IList<TPlugin> GetPlugins<TPlugin>() => GetPluginsAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

        Task<IList<TPlugin>> GetPluginsAsync<TPlugin>(CancellationToken token = default);

        IList<IPlugin> GetPlugins() => GetPluginsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        Task<IList<IPlugin>> GetPluginsAsync(CancellationToken token = default) => GetPluginsAsync<IPlugin>(token);

        void AddPlugin(Type pluginType) => AddPluginAsync(pluginType).ConfigureAwait(false).GetAwaiter().GetResult();

        Task AddPluginAsync(Type pluginType, CancellationToken token = default);

        void AddPlugin<TPlugin>() => AddPluginAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

        Task AddPluginAsync<TPlugin>(CancellationToken token = default) => AddPluginAsync(typeof(TPlugin), token);

        void AddPluginsFrom<TSource>() => AddPluginsFromAsync<TSource>().ConfigureAwait(false).GetAwaiter().GetResult();

        Task AddPluginsFromAsync<TSource>(CancellationToken token = default) => AddPluginsFromAsync(typeof(TSource).Assembly, token);

        void AddPluginsFrom(Assembly sourceAssembly) => AddPluginsFromAsync(sourceAssembly).ConfigureAwait(false).GetAwaiter().GetResult();

        Task AddPluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default);

        void RemovePlugin(Type pluginType) => RemovePluginAsync(pluginType).ConfigureAwait(false).GetAwaiter().GetResult();

        Task RemovePluginAsync(Type pluginType, CancellationToken token = default);

        void RemovePlugin<TPlugin>() => RemovePluginAsync<TPlugin>().ConfigureAwait(false).GetAwaiter().GetResult();

        Task RemovePluginAsync<TPlugin>(CancellationToken token = default) => RemovePluginAsync(typeof(TPlugin), token);

        void RemovePluginsFrom<TSource>() => RemovePluginsFromAsync<TSource>().ConfigureAwait(false).GetAwaiter().GetResult();

        Task RemovePluginsFromAsync<TSource>(CancellationToken token = default) => RemovePluginsFromAsync(typeof(TSource).Assembly, token);

        void RemovePluginsFrom(Assembly sourceAssembly) => RemovePluginsFromAsync(sourceAssembly).ConfigureAwait(false).GetAwaiter().GetResult();

        Task RemovePluginsFromAsync(Assembly sourceAssembly, CancellationToken token = default);
    }
}

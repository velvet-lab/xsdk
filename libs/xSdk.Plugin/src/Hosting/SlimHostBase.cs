using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Hosting
{
    public class SlimHostBase : ISlimHost
    {
        private IServiceProvider _provider = null!;

        public void Configure(IServiceProvider provider)
        {
            _provider = provider;
        }

        public string AppName { get; internal set; } = string.Empty;

        public string AppCompany { get; internal set; } = string.Empty;

        public string AppPrefix { get; internal set; } = string.Empty;

        public string AppVersion { get; internal set; } = string.Empty;

        public IFileSystemService FileSystem => GetService<IFileSystemService>();

        public IVariableService VariableSystem => GetService<IVariableService>();

        public IPluginService PluginSystem => GetService<IPluginService>();

        private TService GetService<TService>()
            where TService : notnull
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("Boot has not been initialized.");
            }

            return _provider.GetRequiredService<TService>();
        }
    }
}

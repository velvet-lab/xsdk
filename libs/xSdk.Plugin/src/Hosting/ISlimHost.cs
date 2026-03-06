using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting
{
    public interface ISlimHost
    {
        string AppName { get; }

        string AppCompany { get; }

        string AppPrefix { get; }

        string AppVersion { get; }

        void Configure(IServiceProvider provider);

        IFileSystemService FileSystem { get; }

        IVariableService VariableSystem { get; }

        IPluginService PluginSystem { get; }
    }
}

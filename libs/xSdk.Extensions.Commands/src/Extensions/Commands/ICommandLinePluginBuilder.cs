using xSdk.Extensions.Plugin;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands
{
    public interface ICommandLinePluginBuilder : IPluginBuilder
    {
        void ConfigureCommandLine(IConfigurator configurator);
    }
}

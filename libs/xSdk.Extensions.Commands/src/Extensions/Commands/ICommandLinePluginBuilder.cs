using Spectre.Console.Cli;
using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Commands;

public interface ICommandLinePluginBuilder : IPluginBuilder
{
    void ConfigureCommandLine(IConfigurator configurator);
}

using Spectre.Console.Cli;
using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Commands;

public abstract class ConsolePluginBuilder : PluginBuilder, IConsolePluginBuilder
{
    public abstract void Configure(IConfigurator builder);
}

using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.Commands;

internal class ChatCommandsPluginBuilder : ConsolePluginBuilder
{
    public override void Configure(IConfigurator builder)
    {
        builder.AddCommand<ClearCommand>(ClearCommand.Definitions.Name);
        builder.AddCommand<ExitCommand>(ExitCommand.Definitions.Name);
    }
}

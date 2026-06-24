using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Variable.Commands;

namespace xSdk.Demos;

internal class ConsoleBuilder() : ConsolePluginBuilder
{
    public override void Configure(IConfigurator builder)
    {
        builder
            .AddDefaultCommands()
            .AddVariableCommands()
            .AddCommand<MyCommand>(MyCommand.Definitions.Name);
    }
}

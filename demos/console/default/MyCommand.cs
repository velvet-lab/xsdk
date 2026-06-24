using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;

namespace xSdk.Demos;

[Description(Definitions.HelpText)]
internal class MyCommand : Command<EmptyCommandSettings>
{
    public static class Definitions
    {
        public const string Name = "my";
        public const string HelpText = "Custom Command";
    }

    protected override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine("[green]Hello from MyCommand![/]");
        return 0;
    }
}

using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

[Description(Definitions.HelpText)]
internal class ExitCommand : Command<EmptyCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "exit";
        public const string HelpText = "Exit the REPL Console";
    }

    public override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        return 0;
    }
}

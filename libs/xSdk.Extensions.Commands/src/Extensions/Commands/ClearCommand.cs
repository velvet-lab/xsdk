using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

[Description(Definitions.HelpText)]
internal class ClearCommand : Command<EmptyCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "clear";
        public const string HelpText = "Clears the last console output";
    }

    public override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        System.Console.Clear();
        return 0;
    }
}

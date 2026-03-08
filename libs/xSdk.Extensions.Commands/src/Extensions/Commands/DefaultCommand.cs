using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

internal class DefaultCommand : Command<DefaultCommandSettings>
{
    public override int Execute(CommandContext context, DefaultCommandSettings settings, CancellationToken cancellationToken)
    {
        return 0;
    }
}

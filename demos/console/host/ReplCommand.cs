using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;

namespace xSdk.Demos;

internal class ReplCommand : Command<EmptyCommandSettings>
{
    protected override int Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        return 0;
    }
}

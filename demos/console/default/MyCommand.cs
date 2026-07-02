using Spectre.Console;
using xSdk.Extensions.Commands;

namespace xSdk.Demos;


internal partial class MyCommand : CommandHandler
{
    public static class Definitions
    {
        public const string Name = "my";
        public const string HelpText = "Custom Command";
    }

    public override Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine("[green]Hello from MyCommand![/]");
        return Task.FromResult(0);
    }
}

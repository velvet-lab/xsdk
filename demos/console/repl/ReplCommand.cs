using Spectre.Console;
using xSdk.Extensions.Commands;

namespace xSdk.Demos;

internal class ReplCommand : CommandHandler
{
    internal static class Definitions
    {
        internal static string Name = "my";
        internal static string Description = "My custom command";
    }

    public override int Execute()
    {
        AnsiConsole.WriteLine("Hello from my custom command!");
        return 0;
    }
}

using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using xSdk.Plugins.Commands;

namespace xSdk.Demos.Commands;

internal class ChatConsoleBuilder : ChatConsolePluginBuilder
{
    public override void CreateBanner()
    {
        AnsiConsole.Write(
            new FigletText("xSDK Chat Console")
                .Color(Color.Green)
                .Centered());
    }

    public override string CreateUserPrompt()
        => AnsiConsole.Ask<string>("Type message to translate: ");

    public override void CreateLastWill()
        => AnsiConsole.WriteLine("Chat console is shutting down. Goodbye!");
}

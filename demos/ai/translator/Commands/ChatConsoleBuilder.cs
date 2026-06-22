using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using xSdk.Extensions.Commands;

namespace xSdk.Demos.Commands;

internal class ChatConsoleBuilder : ConsolePluginBuilder
{
    public override void Configure(IConfigurator builder)
    {
        builder
            .AddDefaultCommands()            
            .AddCommand<ChatCommand>(ChatCommand.Definitions.Name);
    }

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

    public override void CreateHelp(ICommandAppSettings settings, ICommandModel model)
    {
        var helpProvider = new HelpProvider(settings);

        var helpItems = helpProvider.Write(model, null);
        foreach (var item in helpItems.Skip(3))
        {
            AnsiConsole.Write(item);
        }
    }
}

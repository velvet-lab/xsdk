using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Variable.Commands;

namespace xSdk.Demos;

internal class ReplConsoleBuilder() : ConsolePluginBuilder
{
    public override void Configure(IConfigurator builder)
    {
        builder
            .AddDefaultCommands()
            .AddVariableCommands();
    }

    public override void CreateBanner()
    {
        AnsiConsole.Write(
            new FigletText("xSDK REPL Console")
                .Color(Color.Green)
                .Centered());
    }

    public override string CreateUserPrompt()
        => AnsiConsole.Ask<string>("REPL> ");

    public override void CreateLastWill()
        => AnsiConsole.WriteLine("REPL console is shutting down. Goodbye!");

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

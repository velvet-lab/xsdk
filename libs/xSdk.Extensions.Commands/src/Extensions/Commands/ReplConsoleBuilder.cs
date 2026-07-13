using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace xSdk.Extensions.Commands;

public class ReplConsoleBuilder : ConsoleBuilder
{
    public virtual Action? CreateBannerAction { get => field ?? CreateBanner; set; }

    public virtual Action<IList<Command>>? CreateHelpAction { get => field ?? CreateHelp; set; }

    public virtual Action? CreateLastWillAction { get => field ?? CreateLastWill; set; }

    public virtual Func<string>? CreateUserPromptAction { get => field ?? CreateUserPrompt; set; }

    protected override IApplication BuildApplication(IServiceProvider provider)
        => ActivatorUtilities.CreateInstance<ReplApplication>(provider);


    private static void CreateBanner()
    {
        AnsiConsole.Write(
            new FigletText("xSDK REPL Console")
                .Color(Color.Green)
                .Centered());
    }

    private static string CreateUserPrompt()
        => AnsiConsole.Ask<string>("REPL> ");

    private static void CreateLastWill()
        => AnsiConsole.WriteLine("REPL console is shutting down. Goodbye!");

    private static void CreateHelp(IList<Command> commands)
    {
        foreach (Command command in commands)
        {
            AnsiConsole.WriteLine($"Command: {command.Name}");
            AnsiConsole.WriteLine($"Description: {command.Description}");
            AnsiConsole.WriteLine();
        }
    }
}

using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace xSdk.Extensions.Commands;

public abstract class ReplConsolePluginBuilder : ConsolePluginBuilder, IReplConsolePluginBuilder
{
    private const string DEFAULT_PROMPT = "> ";

    public virtual string CreateUserPrompt()
        => DEFAULT_PROMPT;

    public virtual void CreateBanner() { }

    public virtual void CreateHelp(ICommandAppSettings settings, ICommandModel model) { }

    public virtual void CreateLastWill() { }
}

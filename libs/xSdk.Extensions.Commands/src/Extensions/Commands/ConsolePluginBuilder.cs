using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Commands;

public abstract class ConsolePluginBuilder : PluginBuilder, IConsolePluginBuilder
{
    private const string DEFAULT_PROMPT = "> ";

    public abstract void Configure(IConfigurator builder);

    public virtual string CreateUserPrompt()
        => DEFAULT_PROMPT;

    public virtual void CreateBanner() { }

    public virtual void CreateHelp(ICommandAppSettings settings, ICommandModel model) { }

    public virtual void CreateLastWill() { }
}

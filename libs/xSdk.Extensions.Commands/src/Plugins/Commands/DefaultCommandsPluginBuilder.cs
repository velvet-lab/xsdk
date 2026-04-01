using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

internal class DefaultCommandsPluginBuilder : PluginBuilderBase, ICommandsPluginBuilder
{
    private const string Prompt = ">";
    private readonly EnvironmentSetup _environmentSetup;

    public DefaultCommandsPluginBuilder()
    {
        this._environmentSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
    }

    public Func<string> PromptFactory => () =>
    {
        return string.Format("{0} {1} ", _environmentSetup.AppName, Prompt);
    };

    public void Configure(IConfigurator setup)
    {
        setup.SetApplicationName(_environmentSetup.AppName);

        setup.AddVariableCommands();

        setup.AddCommand<ConsoleCommand>(ConsoleCommand.Definitions.Name);
        setup.AddCommand<ClearCommand>(ClearCommand.Definitions.Name);
        setup.AddCommand<ExitCommand>(ExitCommand.Definitions.Name);
    }

    public ICommandApp CreateApplication(IServiceCollection? services = null)
    {
        if (services == null)
        {
            return new CommandApp<DefaultCommand>();
        }
        else
        {
            return new CommandApp<DefaultCommand>(new ServiceRegistrar(services));
        }
    }
}

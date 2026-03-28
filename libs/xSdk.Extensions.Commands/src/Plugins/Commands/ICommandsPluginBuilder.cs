using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.Commands;

public interface ICommandsPluginBuilder : IPluginBuilder
{
    ICommandApp CreateApplication(IServiceCollection? services = null);

    Func<string> PromptFactory { get; }

    void Configure(IConfigurator setup);
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Plugins.Commands;

namespace xSdk.Demos.Builders;

internal class CommandPluginBuilder : PluginBuilderBase, ICommandsPluginBuilder
{
    private readonly EnvironmentSetup _environmentSetup;

    public CommandPluginBuilder(IVariableService variableService)
    {
        this._environmentSetup = variableService.GetSetup<EnvironmentSetup>();
    }


    public Func<string> PromptFactory => throw new NotImplementedException();

    public void Configure(IConfigurator setup)
    {
        setup
            .SetApplicationName(this._environmentSetup.AppName)
            // Tells the command line application to validate all examples before running the application.
            .ValidateExamples();
    }

    public ICommandApp CreateApplication(IServiceCollection? services = null) => throw new NotImplementedException();
}

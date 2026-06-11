/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Commands;

namespace xSdk.Plugins.Commands;

internal sealed class DefaultCommandsPluginBuilder(IOptions<ApplicationOptions> options) : PluginBuilder, ICommandsPluginBuilder
{
    private const string Prompt = ">";

    public Func<string> PromptFactory => () => string.Format("{0} {1} ", options.Value.Name, Prompt);

    public void Configure(IConfigurator setup)
    {
        if (!string.IsNullOrEmpty(options.Value.Name))
        {
            setup.SetApplicationName(options.Value.Name);
        }

        setup.AddVariableCommands();

        setup.AddCommand<ConsoleCommand>(ConsoleCommand.Definitions.Name).IsHidden();
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

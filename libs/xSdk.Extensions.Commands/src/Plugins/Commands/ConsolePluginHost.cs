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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public sealed class ConsolePluginHost<TConsolePluginOptions>(IConsolePluginBuilder builder, ICommandAppBuilder applicationBuilder, IOptions<TConsolePluginOptions> options, IOptions<ApplicationOptions> applicationOptions) : PluginHost
    where TConsolePluginOptions : ConsolePluginOptions, new()
{
    public override void ConfigureLogging(ILogBuilder builder)
    {
        // CLI-Kontext: Nur Warnings, Errors und Critical anzeigen        
        builder.IsLoggingAllowed<ConsoleLoggerProvider>(level => level >= LogLevel.Warning);
    }

    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var registrar = new ServiceRegistrar(services);
        ICommandApp app = applicationBuilder.Build(registrar);
        app
            .Configure(config =>
            {
                if (options.Value.DisableDefaultHelp)
                {
                    config.SetHelpProvider<SilentHelpProvider>();
                }

                if (!string.IsNullOrEmpty(applicationOptions.Value.Name))
                {
                    config.SetApplicationName(applicationOptions.Value.Name);
                }

                builder
                    .Configure(config);
            });

        services            
            .TryAddSingleton<ICommandApp>(provider =>
            {
                registrar.SetServiceProvider(provider);
                return app;
            });
    }
}
